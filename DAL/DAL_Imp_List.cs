using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    public class DAL_Imp_List : IDAL
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

        // Illegal to update ID and specilization Name
        public bool updateSpecilization(Specialization specilization) =>
            updateInList(DataSource.specList, specilization);
        
        public bool addEmployee(Employee employee) =>
            addToList(DataSource.employeeList, employee);

        public bool deleteEmployee(Employee employee) =>
            deleteFromList(DataSource.employeeList, employee);

        public bool updateEmployee(Employee employee) =>
            updateInList(DataSource.employeeList, employee);

        public bool addContract(Contract contract)
        {
            if (contract.contractID == 0) // update contractID if = 0
            {
                uint maxID = 0;
                if (DataSource.contractList.Count != 0)
                {
                    maxID = DataSource.contractList.Max(x => x.contractID);
                }
                else { maxID = 10000000; }

                // update ID
                contract.contractID = maxID;
            }
            // addToList checks if contract already exists in contractList
            return addToList(DataSource.contractList, contract);
        }

        public bool deleteContract(Contract contract) =>
            deleteFromList(DataSource.contractList, contract);

        public bool updateContract(Contract newContract, Contract oldContract) // oldContract needed for finding old Contract in DS by operator==
            => updateInList(DataSource.contractList, newContract, oldContract);



        public bool addEmployer(Employer employer) =>
            addToList(DataSource.employerList, employer);

        public bool deleteEmployer(Employer employer) =>
            deleteFromList(DataSource.employerList, employer);

        public bool updateEmployer(Employer employer) =>
            updateInList(DataSource.employerList, employer);


        public List<Specialization> getSpecilizationList() => DataSource.specList;
        public List<Employee> getEmployeeList()            => DataSource.employeeList;
        public List<Employer> getEmployerList()            => DataSource.employerList;
        public List<Contract> getContractList()            => DataSource.contractList;

        private List<Bank> getTempBanks()
        {
            return new List<Bank>
            {
                new Bank("Leumi", 1155, 19, "20 Havaad Haleumi St", "Jerusalem"),
                new Bank("Hapoalim", 9866, 24, "14 Malki St", "Jerusalem"),
                new Bank("Discount", 8543, 11, "Hagai St", "Jerusalem"),
                new Bank("Bank of Jerusalem", 6654, 8, "Keren Hayesod St", "Jerusalem")
            };
        }
    }
}
