using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class Bank
    {
        public string BankName { get; set; }
        public uint BankNumber { get; set; }
        public uint Branch { get; set; }
        public CivicAddress Address { get; set; }

        public Bank(string _BankName="", uint _BankNumber=0, uint _Branch=0, CivicAddress _Address=null)
        {
            BankName = _BankName;
            BankNumber = _BankNumber;
            Branch = _Branch;
            Address = _Address;
        }

        public static explicit operator Bank(XElement XRoot)
        {
            return new Bank()
            {
                BankNumber = (uint)XRoot.Attribute("BankNumber"),
                BankName = (string)XRoot.Element("BankName"),
                Address = (CivicAddress)XRoot.Element("Address"),
                Branch = (uint)XRoot.Element("Branch")
            };
        }

        public override string ToString()
        {
            return BankName + ", " + BankNumber + ", branch: " + Branch;
        }
    }
}
