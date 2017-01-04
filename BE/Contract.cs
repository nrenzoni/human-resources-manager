﻿using System;
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
