using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    internal class BL_Imp : IBL
    {
        IDAL DAL_Object = FactoryDAL.DALInstance;

        public bool addSpecilization(Specialization specilization)
        {
            return DAL_Object.addSpecilization(specilization);
        }

        public bool deleteSpecilization(Specialization specilization)
        {
            if (DAL_Object.getEmployeeList().Find(e => e.specializationID == specilization.ID) != default(Employee))
                throw new Exception("cannot delete specialization, in use by employee");
            return DAL_Object.deleteSpecilization(specilization);
        }

        public bool updateSpecilization(Specialization specilization)
        {
            return DAL_Object.updateSpecilization(specilization);
        }

        public bool addEmployee(Employee employee)
        {         
            if(DateTime.Today.Year - employee.birthday.Year < 18)
            {
                throw new Exception("employee under legal age of 18");
            }

            var temp = 
                (from item in DAL_Object.getEmployeeList()
                where (item.bankAccountNumber == employee.bankAccountNumber)
                select item).ToList();
            if(temp.Count != 0)
            {
                throw new Exception("Employee with bank account number already exists");
            }

            return DAL_Object.addEmployee(employee);
        }

        public bool deleteEmployee(Employee employee)
        {
            #region check if employee has open contracts
            int openContractCount =
                    (from contr in DAL_Object.getContractList()
                     where contr.EmployeeID == employee.ID && contr.contractFinalized == true && ((DateTime.Today - contr.contractTerminatedDate).Days < 0)
                     select contr).Count();
            if (openContractCount > 0)
                throw new Exception("cannot delete employee with open contract(s)"); 
            #endregion

            return DAL_Object.deleteEmployee(employee);
        }

        public bool updateEmployee(Employee employee) =>
            DAL_Object.updateEmployee(employee);

        public bool addContract(Contract contract)
        {
            #region check if employee and employer exist in DS
            int temp1 = DAL_Object.getEmployeeList().Count(x => x.ID == contract.EmployeeID);
            if (temp1 != 1)
                throw new Exception("cannot add contract for employee that does not exist");

            int temp2 = DAL_Object.getEmployerList().Count(x => x.ID == contract.EmployerID);
                //(from match in DAL_Object.getEmployerList()
                // where match.EmployerID == contract.EmployerID
                // select match).Count();
            if (temp2 != 1)
                throw new Exception("cannot add contract for employer that does not exist");

            #endregion

            #region check if company established less than year ago
            Employer employer = DAL_Object.getEmployerList().Find(x => x.ID == contract.EmployerID);
            if (DateTime.Today.Year - employer.establishmentDate.Year < 1) // company less than 1 year old
            {
                throw new Exception("cannot create contract with company established less than a year ago");
            }
            #endregion

            #region calculate net wage by subtracting commission from gross wage
            int existingContractEmployeeCount =
                (from contr in DAL_Object.getContractList()
                 where contr.EmployeeID == contract.EmployeeID
                 select contr).Count();

            // 1st employee job commission = 10% (existingContractEmployerCount = 0)
            // 2nd employee job commission = 9%  (existingContractEmployerCount = 1)
            // 3rd employee job commission = 8%  ...
            // ...
            // 8th employee job commission = 3%  (existingContractEmployerCount = 7) (minimum commission)
            // nth employee job commission = 3%
            // ...

            double commission;
            if (existingContractEmployeeCount >= 0 && existingContractEmployeeCount < 8)
                commission = 10 - existingContractEmployeeCount; // 10,9,8,7,...,3
            else commission = 3;

            // take commission if company has less than 50 contracts
            int existingContractEmployerCount =
                (from contr in DAL_Object.getContractList()
                 where contr.EmployerID == contract.EmployerID
                 select contr).Count();

            // only take commission for company if they have less than 50 contracts
            if (existingContractEmployerCount < 50)
                commission += (10 - existingContractEmployerCount * .2);

            contract.netWagePerHour = contract.grossWagePerHour - (contract.grossWagePerHour * commission)/100;
            #endregion

            return DAL_Object.addContract(contract);
        }

        public bool deleteContract(Contract contract)
        {
            Contract temp = DAL_Object.getContractList().Find(x => x == contract);
            if ((DateTime.Today - temp.contractTerminatedDate).Days >= 0)
                throw new Exception("cannot delete open contract");

            return DAL_Object.deleteContract(contract);
        }

        public bool updateContract(Contract oldContract, Contract newContract)
        {
            // can only update terminated contract
            Contract temp = DAL_Object.getContractList().Find(x => x == oldContract);
            if ((DateTime.Today - temp.contractTerminatedDate).Days >= 0)
                throw new Exception("cannot update open contract");

            return DAL_Object.updateContract(oldContract, newContract);
        }

        public bool addEmployer(Employer employer)
        {
            // establishment date is in the future
            if ((DateTime.Today - employer.establishmentDate).Days < 0)
                throw new Exception("establishment date of employer cannot be in future");

            // company name already exists
            int matchingCompCount =
                (from comp in DAL_Object.getEmployerList()
                 where comp.companyName == employer.companyName
                 select comp).Count();
            if (matchingCompCount > 0)
                throw new Exception("employer's company name already exists");

            return DAL_Object.addEmployer(employer);
        }

        public bool deleteEmployer(Employer employer)
        {
            // open contracts with employer
            int contrCount =
                (from contr in DAL_Object.getContractList()
                 where contr.EmployerID == employer.ID && ((DateTime.Today - contr.contractTerminatedDate).Days <= 0)
                 select contr).Count();
            if (contrCount > 0)
                throw new Exception("cannot delete employer with open contract");

            return DAL_Object.deleteEmployer(employer);
        }

        public bool updateEmployer(Employer employer)
        {
            return DAL_Object.updateEmployer(employer);
        }

        public IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition)
        {
            if (condition == null) return DAL_Object.getContractList();

            return from contr in DAL_Object.getContractList()
                   where condition(contr) == true
                   select contr;
        }

        public int ContractListByFilterCount(Predicate<Contract> condition) =>
            getContractListByFilter(condition).Count();

        public IEnumerable<IGrouping<Specialization, Contract>> groupContractBySpec(bool ordered = false)
         => from contr in DAL_Object.getContractList()
            let contr_employee = DAL_Object.getEmployeeList().Find(e => e.ID == contr.EmployeeID)
            let contr_spec = DAL_Object.getSpecilizationList().Find(s => s.ID == contr_employee.specializationID)
            orderby // if ordered = true, first order contracts by spec name, then group
                ordered ? contr_spec.specilizationName : 0
            group contr by contr_spec;

        public IEnumerable<IGrouping<string, Contract>> groupContractByEmployeeCity(bool ordered = false)
         => from contr in DAL_Object.getContractList()
            let contr_employee = DAL_Object.getEmployeeList().Find(e => e.ID == contr.EmployeeID)
            let contr_employee_city = contr_employee.address.City
            orderby // if ordered = true, first order contracts by contract employee city, then group
                ordered ? contr_employee_city : default(string) // null
            group contr by contr_employee_city;

        public IEnumerable<IGrouping<string, Contract>> groupContractByEmployerCity(bool ordered = false)
        => from contr in DAL_Object.getContractList()
           let contr_employer = DAL_Object.getEmployerList().Find(e => e.ID == contr.EmployerID)
           let contr_employer_city = contr_employer.address.City
           orderby // if ordered = true, first order contracts by contract employer city, then group
               ordered ? contr_employer_city : default(string) // null
           group contr by contr_employer_city;

        // profit by year of management company
        public IEnumerable<IGrouping<int, double>> getProfitByYear(bool ordered=false) // <int=year (key), double=yearly profit>
        {
            DateTime currDate = DateTime.Today;
            return
                from contr in DAL_Object.getContractList()
                where contr.contractFinalized
                let earlier_end_date = contr.contractTerminatedDate < currDate ? contr.contractTerminatedDate : currDate
                from year in Enumerable.Range(contr.contractEstablishedDate.Year, earlier_end_date.Year)
                let year_profit =
                    // 4 weeks in a month. if not first or last year in contr then 12 months in year, otherwise months in year different.
                    (contr.grossWagePerHour - contr.netWagePerHour) * contr.maxWorkHours * 4 * // profit per month
                    // calculate months in year of contr
                    ( contr.contractEstablishedDate.Year == year ? 13 - contr.contractEstablishedDate.Month : 1 ) * 
                    ( contr.contractTerminatedDate.Year == year ? contr.contractTerminatedDate.Month : 1) *
                    ( contr.contractEstablishedDate.Year !=  year && contr.contractTerminatedDate.Year != year ? 12 : 1)
                group year_profit by year into g_year
                orderby // ordering by year ( possibly could order by profit in year- year.Sum() )
                    ordered ? g_year : null 
                group g_year.Sum() by g_year.Key;
        }



        public List<Specialization> getSpecilizationList()
            => DAL_Object.getSpecilizationList();

        public List<Employee> getEmployeeList()
            => DAL_Object.getEmployeeList();

        public List<Employer> getEmployerList()
            => DAL_Object.getEmployerList();

        public List<Contract> getContractList()
            => DAL_Object.getContractList();
    }
}
