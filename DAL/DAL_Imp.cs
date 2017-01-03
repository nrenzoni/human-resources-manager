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
            else throw new Exception("cannot delete, specilization not found in DS");
        }

        bool update

        public bool addSpecilization(Specialization specilization) => 
            addToList(DataSource.specList, specilization); // returns retuned value of addToList

        public bool deleteSpecilization(Specialization specilization) =>
            deleteFromList(DataSource.specList, specilization);

        // only minWage and maxWage updated. Cannot update ID and specilization Name
        public bool updateSpecilization(Specialization specilization)
        {
            // check if copy or reference. if value is updated in list or just the local copy
            var tmp = DataSource.specList.Where(s => s == specilization).FirstOrDefault();
            if (tmp != null)
            {
                tmp.maxWagePerHour = specilization.maxWagePerHour;
                tmp.minWagePerHour = specilization.minWagePerHour;
                return true;
            }
            else throw new Exception("cannot update specilization, not found in DS");
        }


        public bool addEmployee(Employee employee)
        {

        }

        public bool deleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public bool updateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
