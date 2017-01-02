using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract
    {
        public Contract()
        {
            contractID = contractIDCounter++;
        }

        static uint contractIDCounter = 10000000;
        uint contractID;

        uint EmployerID;
        uint EmployeeID;

        bool isInterviewed; 

        bool contractFinalized; // contract was signed by both parties

        uint grossWagePerHour; // before taxes etc
        uint netWagePerHour; // payment employee receives

        DateTime contractEstablishedDate;
        DateTime contractTerminatedDate;

        uint maxWorkHours;

        public override string ToString()
        {
            return "Contract ID: " + contractID +
                "" // for completion
        }
    }
}
