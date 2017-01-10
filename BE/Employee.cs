using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum Education { student, firstDegree, secondDegree, thirdDegree };

    public class CivicAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
    }

    public class Employee
    {
        // no recommendation in constructor; recommendation assigned using property
        public Employee(uint _ID, string _firstName, string _lastName, DateTime _birthday, uint _phone, CivicAddress _address, uint _yearsOfExperience, string _email, Education _education, bool _armyGraduate, Bank _bank, uint _bankAccountNumber)
        {
            ID = _ID;
            lastName = _lastName;
            firstName = _firstName;
            birthday = _birthday;
            phoneNumber = _phone;
            address = _address;
            yearsOfExperience = _yearsOfExperience;
            email = _email;
            education = _education;
            armyGraduate = _armyGraduate;
            bank = _bank;
            bankAccountNumber = _bankAccountNumber;
        }

        public uint ID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public DateTime birthday { get; set; }
        public uint phoneNumber { get; set; }
        public CivicAddress address { get; set; }
        public uint yearsOfExperience { get; set; }
        public string recommendationNotes { get; set; }
        public string email { get; set; }
        public Education education { get; set; }
        public bool armyGraduate { get; set; }

        public Bank bank { get; set; }
        public uint bankAccountNumber { get; set; }

        public uint specializationID { get; set; }


        public override string ToString()
        {
            return  lastName + ", " + firstName + ". ID: " + ID + ", birthday: " + birthday.ToString("d") + ", Phone: " +
                    phoneNumber + " address: " + address + ", Years of Experience: " + yearsOfExperience + ", Education: " +
                    education.ToString() + ((armyGraduate) ? " served in Army" : "did not serve in Army") + ", recommendation Notes: " +
                    recommendationNotes + ", Bank Account Info: " + bankAccount.ToString();
        }

        public static bool operator ==(Employee e1, Employee e2)
        {
            return e1.specializationID == e2.specializationID;
        }

        public static bool operator !=(Employee e1, Employee e2)
        {
            return e1.specializationID != e2.specializationID;
        }
    }
}
