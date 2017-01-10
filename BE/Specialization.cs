using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum SpecializationName { Programming, Communications, SoftwareSecurity, GraphicDesign };

    public class Specialization
    {
        public uint ID { get; }
        public SpecializationName specilizationName { get; }
        public double minWagePerHour { get; set; }
        public double maxWagePerHour { get; set; }

        public Specialization(SpecializationName specName, double minWage, double maxWage)
        {
            ID = (uint)specName;
            specilizationName = specName;
            minWagePerHour = minWage;
            maxWagePerHour = maxWage;
        }

        #region Operators
        public static bool operator ==(Specialization s1, Specialization s2)
        {
            return s1.ID == s2.ID;
        }

        public static bool operator !=(Specialization s1, Specialization s2)
        {
            return s1.ID != s2.ID;
        } 
        #endregion

        public override string ToString()
        {
            return "Specialty: " + SpecializationField + " min wage: " + minWage + ", max wage: " + maxWage + "\n";
        }
    }
}
///