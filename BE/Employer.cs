using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employer
    {
        public Employer(uint ID, string companyname, uint phoneNum, string addr, SpecializationName specName, DateTime estabDate, string firstname="", string lastname="", bool isPrivate=false)
        {
            privatePerson = isPrivate;
            EmployerID = ID;
            firstName = firstname;
            lastName = lastname;
            companyName = companyname;
            phoneNumber = phoneNum;
            address = addr;
            specializationName = specName;
            establishmentDate = estabDate;
        }

        // if not private person then company
        bool privatePerson;

        readonly uint EmployerID;

        // if privatePerson
        string firstName, lastName;

        // optional if private person
        string companyName;

        uint phoneNumber;
        string address;
        
        SpecializationName specializationName; // enum

        DateTime establishmentDate;

        public override string ToString()
        {
            return ((privatePerson) ? "Private Employer" : "Company") +
                " ID: " + EmployerID +
                ((privatePerson) ? " Name: " +
                lastName + ", " + firstName : "") +
                ((string.IsNullOrEmpty(companyName)) ? "" : " Company Name: " + companyName) +
                " Phone Number: " + phoneNumber +
                " Address: " + address; 
        }

        public static bool operator ==(Employer e1, Employer e2)
        {
            return e1.EmployerID == e2.EmployerID;
        }

        public static bool operator !=(Employer e1, Employer e2)
        {
            return e1.EmployerID != e2.EmployerID;
        }
    }
}
