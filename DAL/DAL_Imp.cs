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
        public bool addSpecilization(Specialization specilization)
        {
            if (DataSource.specList.Contains(specilization))
                throw new Exception("specilization already exists in DS");

            DataSource.specList.Add(specilization);
            return true;
        }

        public bool deleteSpecilization(Specialization specilization)
        {
            int foundIndex = DataSource.specList.FindIndex(s => s == specilization); // comparision based on specilizationID
            if (foundIndex != -1)
            {
                DataSource.specList.RemoveAt(foundIndex);
                return true;
            }
            else throw new Exception("cannot delete, specilization not found in DS");
        }

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
    }
}
