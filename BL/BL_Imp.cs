using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;
using System.Reflection;

namespace BL
{
    internal class BL_Imp : IBL
    {
        IDAL DAL_Object = FactoryDAL.DALInstance;

        // returns true if obj has property with no value or null
        bool hasEmptyFields(object obj, params string[] exclude)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                // if bool type, skip, because default of bool is false and that is a valid value
                if (property.PropertyType == typeof(bool))
                    continue;

                // skip if type is enum
                if (property.PropertyType.IsEnum)
                    continue;


                // if current property's name matches one of exclude params, skip
                bool skip = false;
                foreach (string excludeRule in exclude)
                {
                    if (property.Name == excludeRule)
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip)
                    continue;

                // only check properties which have a set method, since only those are stored in DS
                if(property.GetSetMethod() != null)
                {
                    // if property is of type civicAddress or Bank, recursively check its properties
                    if (property.PropertyType == typeof(CivicAddress) || property.PropertyType == typeof(Bank))
                    {
                        if (hasEmptyFields(property.GetValue(obj), exclude) == true)
                            return true;
                        else
                            continue;
                    }

                    object value = property.GetValue(obj, null);

                    // if obj is equal to its type's default or equal to NULL, return true
                    if ( Equals(value, getDefault(property.PropertyType)) )
                        return true;
                }
            }

            // if no empty or null properties found, return false
            return false;
        }

        static object getDefault(Type type)
        {
            if (type.IsValueType) // type is a struct type
                return Activator.CreateInstance(type);
            else if (type == typeof(string))
                return "";
            else
                return null;
        }

        public bool addSpecialization(Specialization specialization)
        {
            if (hasEmptyFields(specialization))
                throw new Exception("please fill out all fields");

            if (DAL_Object.getSpecilizationList().Exists(s => s.specializationName.Trim().ToLower() == specialization.specializationName.Trim().ToLower()))
                throw new Exception("a specialization already exists with name" + specialization.specializationName);

            // verify max wage is greater than min wage
            if (specialization.maxWagePerHour <= specialization.minWagePerHour)
                throw new Exception("max wage is under min wage");

            return DAL_Object.addSpecilization(specialization);
        }

        public bool deleteSpecilization(Specialization specilization)
        {
            if (DAL_Object.getEmployeeList().Exists(e => e.specializationID == specilization.ID) == true)
                throw new Exception("cannot delete specialization, in use by employee");

            if(DAL_Object.getEmployerList().Exists(e=> e.specializationID == specilization.ID) == true)
                throw new Exception("cannot delete specialization, in use by employer");

            return DAL_Object.deleteSpecilization(specilization);
        }

        public bool updateSpecilization(Specialization specilization)
        {
            if (specilization.maxWagePerHour <= specilization.minWagePerHour)
                throw new Exception("max wage is under min wage");

            return DAL_Object.updateSpecilization(specilization);
        }

        public bool addEmployee(Employee employee)
        {
            if (hasEmptyFields(employee, "recommendationNotes", "specializationID", "BankNumber")) // recommendationNotes are optional, specializationID is uint and 0 is valid, and BankNumber is always empty in employee, but employee has Bank struct
                throw new Exception("please fill out all fields");

            if (DateTime.Today.Year - employee.birthday.Year < 18)
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

        public uint getNextSpecID()
            => DAL_Object.getNextSpecID();

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
            if (hasEmptyFields(contract, "netWagePerHour")) // netwage calculated below in BL
                throw new Exception("please fill out all fields");

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

            #region check if gross wage per hour is within range of employee's specialization min/max wage

            Employee contract_employee = DAL_Object.getEmployeeList().Find(x => x.ID == contract.EmployeeID);
            Specialization contract_spec =  
                DAL_Object.getSpecilizationList().Find(s => s.ID == contract_employee.specializationID);
            if (contract.grossWagePerHour < contract_spec.minWagePerHour
                || contract.grossWagePerHour > contract_spec.maxWagePerHour)
                throw new Exception("gross wage per hour not within range of min/max of " + contract_spec.ToString());

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
            Contract foundContr = DAL_Object.getContractList().Find(x => Equals(x,contract));
            if (foundContr == null)
                return false; // contract does not exist in DB

            if (DateTime.Now < foundContr.contractTerminatedDate) // previous termination date in future
            {
                contract.contractTerminatedDate = DateTime.Today;
                DAL_Object.updateContract(contract);
                return true;
            }
            else // contract already terminated
                throw new Exception("Contract already terminated");
        }

        public uint getNextContractID()
            => DAL_Object.getNextContractID();

        public bool addEmployer(Employer employer)
        {
            // if private employer, all properties must be filled, including first name and last name
            if (employer.privatePerson && hasEmptyFields(employer))
                throw new Exception("please fill out all fields");

            // if not private, skip check of firstName and lastName
            else if(hasEmptyFields(employer, "firstName", "lastName"))
                throw new Exception("please fill out all fields");


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
                ordered ? contr_spec.specializationName : null
            select new ContractGroupingContainer { key = contr_spec, contract = contr };

        public IEnumerable<ContractGroupingContainer> groupContractByEmployerCity(bool ordered = false)
        => from contr in DAL_Object.getContractList()
           let contr_employer = DAL_Object.getEmployerList().Find(e => e.ID == contr.EmployerID)
           let contr_employer_addr = contr_employer.address
           orderby // if ordered = true, first order contracts by contract employer city, then address, then group
               ordered ? contr_employer_addr.City : null,
               ordered ? contr_employer_addr.Address : null
           select new ContractGroupingContainer { key = contr_employer_addr.City, contract = contr };

        public IEnumerable<ContractGroupingContainer> groupContractByEmployeeCity(bool ordered = false)
         => from contr in DAL_Object.getContractList()
            let contr_employee = DAL_Object.getEmployeeList().Find(e => e.ID == contr.EmployeeID)
            let contr_employee_city_addr = contr_employee.address
            orderby // if ordered = true, first order contracts by contract employee city, then group
                ordered ? contr_employee_city_addr.City : null,
                ordered ? contr_employee_city_addr.Address : null
            select new ContractGroupingContainer { key = contr_employee_city_addr.City, contract=contr };

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

        public IEnumerable<IGrouping<string,Bank>> getBanksGrouped()
            => from bank in DAL_Object.getBankList()
                group bank by bank.BankName;

        public List<Specialization> getSpecilizationList()
            => new List<Specialization>(DAL_Object.getSpecilizationList());

        public List<Employee> getEmployeeList()
            => new List<Employee>(DAL_Object.getEmployeeList());

        public List<Employer> getEmployerList()
            => new List<Employer>(DAL_Object.getEmployerList());

        public List<Contract> getContractList()
            => new List<Contract>(DAL_Object.getContractList());

        public IEnumerable<ContractGroupingContainer> getContractsInContainer()
            => from contr in DAL_Object.getContractList()
               select new ContractGroupingContainer { key = null, contract = contr };

        public List<Bank> getBankList()
            => DAL_Object.getBankList();

        public DoWorkDelegate getXMLBankBackground_DoWork()
        => DAL_Object.getXMLBankBackground_DoWork();

    }

}
