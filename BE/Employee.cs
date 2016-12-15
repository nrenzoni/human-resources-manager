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
        Education education;
        bool armyGraduate;
        Bank bankAccount;
        Specialization specialty;

        enum Education { student, firstDegree, secondDegree, thirdDegree };



    }
}
