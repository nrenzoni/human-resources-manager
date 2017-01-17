using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum SpecializationName { Programming, Communications, Software_Security, Graphics_Design };

    public class Specialization
    {
        public uint ID { get; private set; }
        SpecializationName _specilizationName;
        public SpecializationName specilizationName
        { get { return _specilizationName; }
            set
            {
                _specilizationName = value;
                ID = (uint)value;
            }
        }
        public double minWagePerHour { get; set; }
        public double maxWagePerHour { get; set; }

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
            => "ID: " + ID +
            ", Specialty: " + specilizationName.ToString() +
            ", min wage per hour: " + minWagePerHour +
            ", max wage per hour: " + maxWagePerHour;
    }
}