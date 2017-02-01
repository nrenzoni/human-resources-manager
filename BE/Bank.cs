using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bank
    {
        public string BankName { get; set; }
        public uint BankNumber { get; set; }
        public uint Branch { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public Bank(string _BankName, uint _BankNumber, uint _Branch, string _Address, string _City)
        {
            BankName = _BankName;
            BankNumber = _BankNumber;
            Branch = _Branch;
            Address = _Address;
            City = _City;
        }

        //public override string ToString()
        //{
        //    return base.ToString();
        //}
    }
}
