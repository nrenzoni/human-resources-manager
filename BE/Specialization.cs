using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum SpecializationName { Programming=10000000, Communications, SoftwareSecurity, GraphicDesign };

    public class Specialization
    {
        //static uint specilizationIDCounter = 10000000;

        public uint specilizationID { get; }
        public SpecializationName specilizationName { get; }
        public double minWagePerHour { get; set; }
        public double maxWagePerHour { get; set; }

        public Specialization(SpecializationName specName, double minWage, double maxWage)
        {
            specilizationID = (uint)specName;
            specilizationName = specName;
            minWagePerHour = minWage;
            maxWagePerHour = maxWage;
        }

        public static bool operator==(Specialization s1, Specialization s2)
        {
            return s1.specilizationID == s2.specilizationID;
        }

        public static bool operator !=(Specialization s1, Specialization s2)
        {
            return s1.specilizationID != s2.specilizationID;
        }

        public override string ToString()
        {
            return "Specialty: " + SpecializationField + " min wage: " + minWage + ", max wage: " + maxWage + "\n";
        }
    }
}
///