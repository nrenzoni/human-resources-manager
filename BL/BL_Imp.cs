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

        bool addSpecilization(Specialization specilization)
        {

        }

        bool deleteSpecilization(Specialization specilization);
        bool updateSpecilization(Specialization specilization);

        bool addEmployee(Employee employee)
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

        bool deleteEmployee(Employee employee);
        bool updateEmployee(Employee employee);

        bool addContract(Contract contract)
        {
            int temp1 =
                (from match in DAL_Object.getEmployeeList()
                 where match.ID == contract.EmployeeID
                 select match).ToList().Count;

            if (temp1 != 1)
                throw new Exception("cannot add contract for employee that does not exist");

            int temp2 =
                (from match in DAL_Object.getEmployerList()
                 where match.EmployerID == contract.EmployerID
                 select match).ToList().Count;

            if (temp2 != 1)
                throw new Exception("cannot add contract for employer that does not exist");

            return DAL_Object.addContract(contract);
        }

        bool deleteContract(Contract contract);
        bool updateContract(Contract oldContract, Contract newContract);

        bool addEmployer(Employer employer);
        bool deleteEmployer(Employer employer);
        bool updateEmployer(Employer employer);
    }
}
