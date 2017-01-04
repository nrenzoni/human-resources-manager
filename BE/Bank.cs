using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bank
    {
        string BankName { get; set; }
        uint BankNumber { get; set; }
        uint Branch { get; set; }
        string Address { get; set; }
        string City { get; set; }

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
