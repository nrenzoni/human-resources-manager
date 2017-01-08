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
        IDAL DAL_Object = FactoryDAL.getDALInstance;

        public bool addSpecilization(Specialization specilization)
        {
            return DAL_Object.addSpecilization(specilization);
        }

        public bool deleteSpecilization(Specialization specilization)
        {
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

            int temp1 =
                (from match in DAL_Object.getEmployeeList()
                 where match.ID == contract.EmployeeID
                 select match).Count();

            if (temp1 != 1)
                throw new Exception("cannot add contract for employee that does not exist");

            int temp2 =
                (from match in DAL_Object.getEmployerList()
                 where match.EmployerID == contract.EmployerID
                 select match).Count();

            if (temp2 != 1)
                throw new Exception("cannot add contract for employer that does not exist");

            #endregion

            #region check if company established less than year ago
            Employer employer = DAL_Object.getEmployerList().Find(x => x.EmployerID == contract.EmployerID);
            if (DateTime.Today.Year - employer.establishmentDate.Year < 1) // company less than 1 year old
            {
                throw new Exception("cannot create contract with company established less than a year ago");
            }
            #endregion

            #region calculate net wage for person by subtracting commission from gross wage

            int existingContractEmployeeCount =
                (from contr in DAL_Object.getContractList()
                 where contr.EmployeeID == contract.EmployeeID
                 select contr).ToList().Count();

            // 1st time commission = 10% (existingContractEmployerCount = 0)
            // 2nd time commission = 9%
            // 3rd time commission = 8%
            // ...
            // 8th time commission = 3% (minimum commission)
            // nth time commission = 3%
            // ...

            int commissionFromEmployee;
            if (existingContractEmployeeCount >= 0 && existingContractEmployeeCount < 8)
                commissionFromEmployee = 10 - existingContractEmployeeCount;
            else commissionFromEmployee = 3;



            contract.netWagePerHour = contract.grossWagePerHour - (contract.grossWagePerHour * commissionFromEmployee)/100;
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
                 where contr.EmployerID == employer.EmployerID && ((DateTime.Today - contr.contractTerminatedDate).Days <= 0)
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
            return from contr in DAL_Object.getContractList()
                   where condition(contr) == true
                   select contr;
        }

        public int ContractListByFilterCount(Predicate<Contract> condition) =>
            getContractListByFilter(condition).Count();

        public IEnumerable<IGrouping<Specialization, Contract>> groupContractBySpec(bool ordered = false)
        {

        }

        public IEnumerable<IGrouping<string, Contract>> groupContractByEmployerCity(bool ordered = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, Contract>> groupContractByEmployeeCity(bool ordered = false)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IGrouping<Employee, Contract>> IBL.groupContractByEmployerCity(bool ordered)
        {
            throw new NotImplementedException();
        }
    }
}
