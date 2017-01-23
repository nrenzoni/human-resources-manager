using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract
    {        
        public uint contractID { get; set; }

        public uint EmployerID { get; set; }
        public uint EmployeeID { get; set; }


        public bool isInterviewed { get; set; }

        public bool contractFinalized { get; set; } // contract was signed by both parties

        public double grossWagePerHour { get; set; } // before taxes etc
        public double netWagePerHour { get; set; } // payment employee receives

        public double profit { get { return grossWagePerHour - netWagePerHour; } }

        public DateTime contractEstablishedDate { get; set; }
        public DateTime contractTerminatedDate { get; set; }


        public uint maxWorkHours { get; set; } // per week

        public override string ToString()
            => "ID: " + contractID + ", Employer ID: " + EmployerID + ", Employee ID: " + EmployeeID + " " +
            (isInterviewed ? "was" : "was not") + " interviewed" +
            ", contract " + (contractFinalized ? "is finalized" : "is not finalized") +
            ", gross wage per hour: " + grossWagePerHour + ", net wage per hour: " + netWagePerHour +
            ", contract established date: " + contractEstablishedDate + ", contract " +
            ((DateTime.Today.Date - contractTerminatedDate.Date).Days > 0 ? "terminated" : "terminates") + " on: "
            + contractTerminatedDate.ToShortDateString() +
            ", max work hours: " + maxWorkHours;


        public static bool operator ==(Contract c1, Contract c2)
        {
            return c1.contractID == c2.contractID;
        }

        public static bool operator !=(Contract c1, Contract c2)
        {
            return c1.contractID != c2.contractID;
        }
    }
}
