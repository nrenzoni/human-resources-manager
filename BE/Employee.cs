﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum Education { student, firstDegree, secondDegree, thirdDegree };

    public class Employee
    {
        // needs implementation
        public Employee(string _firstName, string _lastName, DateTime _birthday, uint _ID, string _address, uint _yearsOfExperience, string _email, Education _education, bool _armyGraduate, Bank _bank, uint _bankAccountNumber)
        {

        }

        readonly uint ID;
        string lastName, firstName;
        DateTime birthday;
        int phoneNumber;
        string address;
        uint yearsOfExperience;
        string recommendationNotes;
        string email;
        Education education;
        bool armyGraduate;

        Bank bankAccount;
        uint bankAccountNumber { get; }

        //Specialization specialty; (removed)
        uint specializationID; // specializationID also exists in Specialazation class


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
