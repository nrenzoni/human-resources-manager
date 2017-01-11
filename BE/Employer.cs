using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employer
    {
        //public Employer(uint ID, string companyname, uint phoneNum, CivicAddress addr, SpecializationName specName, DateTime estabDate, string firstname="", string lastname="", bool isPrivate=false)
        //{
        //    privatePerson = isPrivate;
        //    this.ID = ID;
        //    firstName = firstname;
        //    lastName = lastname;
        //    companyName = companyname;
        //    phoneNumber = phoneNum;
        //    address = addr;
        //    specializationName = specName;
        //    establishmentDate = estabDate;
        //}

        // if not private person then company
        public bool privatePerson { get; set; }

        public uint ID { get; set; }

        // if privatePerson
        public string firstName { get; set; }
        public string lastName{ get; set; }

        // optional if private person
        public string companyName { get; set; }

        public uint phoneNumber { get; set; }
        public CivicAddress address { get; set; }

        public SpecializationName specializationName { get; set; } // enum

        public DateTime establishmentDate { get; set; }

        public override string ToString()
        {
            return ((privatePerson) ? "Private Employer" : "Company") +
                " ID: " + ID +
                ((privatePerson) ? " Name: " +
                lastName + ", " + firstName : "") +
                ((string.IsNullOrEmpty(companyName)) ? "" : " Company Name: " + companyName) +
                " Phone Number: " + phoneNumber +
                ", " + address;
        }

        public static bool operator ==(Employer e1, Employer e2)
        {
            return e1.ID == e2.ID;
        }

        public static bool operator !=(Employer e1, Employer e2)
        {
            return e1.ID != e2.ID;
        }
    }
}
