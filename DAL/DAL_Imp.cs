using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    public class DAL_Imp : IDAL
    {
        bool addToList<T>(List<T> list, T element)
        {
            if(list.Contains(element))
                throw new Exception(element.GetType() + "already exists in " + nameof(list));
            list.Add(element);
            return true;
        }

        bool deleteFromList<T>(List<T> list, T element) where T : class
        {
            int foundIndex = list.FindIndex(x => x == element); // comparision based on specilizationID
            if (foundIndex != -1)
            {
                DataSource.specList.RemoveAt(foundIndex);
                return true;
            }
            else throw new Exception("cannot delete, " + nameof(element) + " not found in " + nameof(list));
        }

        /// act performs action on matching element in list
        bool updateInList<T>(List<T> list, T newElement, T oldElement=null) where T : class
        {
            try
            {
                if (oldElement == null)
                {
                    bool returnVal = deleteFromList(list, newElement);
                    addToList(list, newElement);
                    return returnVal;
                }
                else // updateContract uses oldElement as well
                {
                    bool returnVal = deleteFromList(list, oldElement);
                    addToList(list, newElement);
                    return returnVal;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("DS update fail: " + ex.ToString());
            }
        }

        public bool addSpecilization(Specialization specilization) => 
            addToList(DataSource.specList, specilization); // returns retuned value of addToList

        public bool deleteSpecilization(Specialization specilization) =>
            deleteFromList(DataSource.specList, specilization);

        // only minWage and maxWage updated. Cannot update ID and specilization Name
        public bool updateSpecilization(Specialization specilization)
        {
            return updateInList(DataSource.specList, specilization);
        }
        
        public bool addEmployee(Employee employee) =>
            addToList(DataSource.employeeList, employee);

        public bool deleteEmployee(Employee employee) =>
            deleteFromList(DataSource.employeeList, employee);

        public bool updateEmployee(Employee employee) =>
            updateInList(DataSource.employeeList, employee);

        public bool addContract(Contract contract)
        {
            return addToList(DataSource.contractList, contract);
        }

        public bool deleteContract(Contract contract)
        {
            return deleteFromList(DataSource.contractList, contract);
        }

        public bool updateContract(Contract newContract, Contract oldContract) // oldContract needed for finding old Contract in DS by operator==
        {
            return updateInList(DataSource.contractList, newContract, oldContract);
        }



        public bool addEmployer(Employer employer) =>
            addToList(DataSource.employerList, employer);

        public bool deleteEmployer(Employer employer) =>
            deleteFromList(DataSource.employerList, employer);

        public bool updateEmployer(Employer employer) =>
            updateInList(DataSource.employerList, employer);


        List<Specialization> getSpecilizationList() => DataSource.specList;
        List<Employee> getEmployeeList()            => DataSource.employeeList;
        List<Employer> getEmployerList()            => DataSource.employerList;
        List<Contract> getContractList()            => DataSource.contractList;


    }
}
