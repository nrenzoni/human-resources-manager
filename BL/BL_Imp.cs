using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using System.Collections.ObjectModel;

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
            if (DAL_Object.getEmployeeList().Exists(e => e.specializationID == specilization.ID) != true)
                throw new Exception("cannot delete specialization, in use by employee");

            if(DAL_Object.getEmployerList().Exists(e=> e.specializationName == specilization.specilizationName) != true)
                throw new Exception("cannot delete specialization, in use by employer");

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
            if (DAL_Object.getEmployeeList().Count(x => x.ID == contract.EmployeeID) != 1)
                throw new Exception("cannot add contract for employee that does not exist");
            
            if (DAL_Object.getEmployerList().Count(x => x.ID == contract.EmployerID) != 1)
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

        public bool terminateContract(Contract contract)
        {
            // can only update terminated contract
            Contract foundContr = DAL_Object.getContractList().Find(x => Equals(x,contract));
            if (DateTime.Now < foundContr.contractTerminatedDate) // previous termination date in future
            {
                foundContr.contractTerminatedDate = DateTime.Now;
                return true;
            }
            else // contract already terminated
                throw new Exception("Contract already terminated");

            //return DAL_Object.updateContract(oldContract, newContract);
        }

        public uint getNextContractID()
            => DAL_Object.getNextContractID();

        public bool addEmployer(Employer employer)
        {
            // establishment date is in the future
            if (DateTime.Today < employer.establishmentDate)
                throw new Exception("establishment date of employer cannot be in future");

            // company name already exists
            int matchingCompCount =
                (from comp in DAL_Object.getEmployerList()
                 where comp.companyName == employer.companyName
                 select comp).Count();
            if (matchingCompCount > 0)
                throw new Exception("Company name already exists");

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

        public IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition = null)
        { 
            return condition == null ? 
                DAL_Object.getContractList() :
                from contr in DAL_Object.getContractList()
                where condition(contr) == true
                select contr;
        }
        public IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition, out int count)
        {
            var filtered = from contr in DAL_Object.getContractList()
                       where condition(contr) == true
                       select contr;
            count = filtered.Count();
            return filtered;
        }

        public IEnumerable<ContractGroupingContainer> groupContractByEmployeeSpec(bool ordered = false)
         => from contr in DAL_Object.getContractList()
            let contr_employee = DAL_Object.getEmployeeList().Find(e => e.ID == contr.EmployeeID)
            let contr_spec = DAL_Object.getSpecilizationList().Find(s => s.ID == contr_employee.specializationID)
            orderby // if ordered = true, first order contracts by spec name, then group
                ordered ? contr_spec.specilizationName : 0
            select new ContractGroupingContainer { key = contr_spec, contract = contr };

        public IEnumerable<ContractGroupingContainer> groupContractByEmployerCity(bool ordered = false)
        => from contr in DAL_Object.getContractList()
           let contr_employer = DAL_Object.getEmployerList().Find(e => e.ID == contr.EmployerID)
           let contr_employer_addr = contr_employer.address
           orderby // if ordered = true, first order contracts by contract employer city, then address, then group
               ordered ? contr_employer_addr.City : null,
               ordered ? contr_employer_addr.Address : null
           select new ContractGroupingContainer { key = contr_employer_addr.City, contract = contr };

        public IEnumerable<IGrouping<string, Contract>> groupContractByEmployeeCity(bool ordered = false)
         => from contr in DAL_Object.getContractList()
            let contr_employee = DAL_Object.getEmployeeList().Find(e => e.ID == contr.EmployeeID)
            let contr_employee_city_addr = contr_employee.address
            orderby // if ordered = true, first order contracts by contract employee city, then group
                ordered ? contr_employee_city_addr.City : null,
                ordered ? contr_employee_city_addr.Address : null
            group contr by contr_employee_city_addr.City;

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
            => new List<Specialization>(DAL_Object.getSpecilizationList());

        public List<Employee> getEmployeeList()
            => new List<Employee>(DAL_Object.getEmployeeList());

        public List<Employer> getEmployerList()
            => new List<Employer>(DAL_Object.getEmployerList());

        public List<Contract> getContractList()
            => new List<Contract>(DAL_Object.getContractList());
    }
}
