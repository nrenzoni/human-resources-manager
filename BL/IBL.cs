using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    interface IBL
    {
        bool addSpecilization(Specialization specilization);
        bool deleteSpecilization(Specialization specilization);
        bool updateSpecilization(Specialization specilization);

        bool addEmployee(Employee employee);
        bool deleteEmployee(Employee employee);
        bool updateEmployee(Employee employee);

        bool addContract(Contract contract);
        bool deleteContract(Contract contract);
        bool updateContract(Contract oldContract, Contract newContract);

        bool addEmployer(Employer employer);
        bool deleteEmployer(Employer employer);
        bool updateEmployer(Employer employer);
    }
}
