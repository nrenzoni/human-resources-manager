using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract
    {
        // need to finish
        public Contract(uint _EmployeID, uint _EmployeeID, bool _isInterviewed, bool contractFinalized, )
        {

        }
        
        public uint contractID { get; set; }

        public uint EmployerID { get; set; }
        public uint EmployeeID { get; set; }


        public bool isInterviewed { get; set; }

        public bool contractFinalized { get; set; } // contract was signed by both parties

        public uint grossWagePerHour { get; set; } // before taxes etc
        public uint netWagePerHour { get; set; } // payment employee receives

        public DateTime contractEstablishedDate { get; set; }
        public DateTime contractTerminatedDate { get; set; }

        public uint maxWorkHours { get; set; }

        public override string ToString()
        {
            return "Contract ID: " + contractID +
                "" // for completion
        }

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
