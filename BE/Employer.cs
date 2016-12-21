using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class Employer
    {
        // if not private person then company
        bool privatePerson;

        uint EmployerID;

        // if privatePerson
        string firstName, lastName;

        // optional if private person
        string companyName;

        uint phoneNumber;
        string address;

        // if Employer has a field of type Specialization, then can use SpecializationField property to return value for specializationName
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
    }
}
