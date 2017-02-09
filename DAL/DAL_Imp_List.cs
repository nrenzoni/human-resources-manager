using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
using System.ComponentModel;

namespace DAL
{
    public class DAL_Imp_List : IDAL
    {
        bool addToList<T>(List<T> list, T element)
        {
            if(list.Contains(element)) // uses 
                throw new Exception(element.ToString() + " already exists in " + nameof(list));
            list.Add(element);
            return true;
        }

        bool deleteFromList<T>(List<T> list, T element) where T : class
        {
            int foundIndex = list.FindIndex(x => Equals(x,element)); // comparision based on specilizationID
            if (foundIndex != -1)
            {
                list.RemoveAt(foundIndex);
                return true;
            }
            else throw new Exception("cannot delete, " + nameof(element) + " not found in " + nameof(list));
        }

        // only need oldElement for updateContract function (new ID)
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

        public bool addSpecilization(Specialization specilization)
        {
            specilization.ID = getNextSpecID();
            return addToList(List_Source.specList, specilization); // returns retuned value of addToList
        }

        public uint getNextSpecID()
            => List_Source.specList.Count != 0 ?
                List_Source.specList.Max(s => s.ID) + 1 : 10000000;

        public bool deleteSpecilization(Specialization specilization) =>
            deleteFromList(List_Source.specList, specilization);

        // Illegal to update ID and specilization Name
        public bool updateSpecilization(Specialization specilization) =>
            updateInList(List_Source.specList, specilization);
        
        public bool addEmployee(Employee employee) =>
            addToList(List_Source.employeeList, employee);

        public bool deleteEmployee(Employee employee) =>
            deleteFromList(List_Source.employeeList, employee);

        public bool updateEmployee(Employee employee) =>
            updateInList(List_Source.employeeList, employee);

        public bool addContract(Contract contract, bool autoAssignID = true)
        {
            if(autoAssignID)
            {
                contract.contractID = getNextContractID();
            }

            // addToList checks if contract already exists in contractList, however, since nextID is unique, will always work
            return addToList(List_Source.contractList, contract);
        }

        public bool deleteContract(Contract contract) =>
            deleteFromList(List_Source.contractList, contract);

        public bool updateContract(Contract contract)
        {
            throw new NotImplementedException();
        }

        public uint getNextContractID()
            => List_Source.contractList.Count != 0 ?
            List_Source.contractList.Max(x => x.contractID) + 1 : 10000000;

        public bool addEmployer(Employer employer) =>
            addToList(List_Source.employerList, employer);

        public bool deleteEmployer(Employer employer) =>
            deleteFromList(List_Source.employerList, employer);

        public bool updateEmployer(Employer employer) =>
            updateInList(List_Source.employerList, employer);


        public List<Specialization> getSpecilizationList() => List_Source.specList;
        public List<Employee> getEmployeeList()            => List_Source.employeeList;
        public List<Employer> getEmployerList()            => List_Source.employerList;
        public List<Contract> getContractList()            => List_Source.contractList;
        public List<Bank> getBankList()
        {
            throw new NotImplementedException();
        }


        public DoWorkDelegate getXMLBankBackground_DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
