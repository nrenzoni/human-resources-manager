using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employee
    {
        int ID;
        string lastName, firstName;
        DateTime birthday;
        int phoneNumber;
        string address;
        uint yearsOfExperience;
        string recommendationNotes;
        Education education;
        bool armyGraduate;
        Bank bankAccount;

        Specialization specialty;
        uint specializationID; // specializationID also exists in Specialazation class

        enum Education { student, firstDegree, secondDegree, thirdDegree };

        public override string ToString()
        {
            return  lastName + ", " + firstName + ". ID: " + ID + ", birthday: " + birthday.ToString("d") + ", Phone: " +
                    phoneNumber + " address: " + address + ", Years of Experience: " + yearsOfExperience + ", Education: " +
                    education.ToString() + ((armyGraduate) ? " served in Army" : "did not serve in Army") + ", recommendation Notes: " +
                    recommendationNotes + ", Bank Account Info: " + bankAccount.ToString();
        }

    }
}
