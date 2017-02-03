using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IDAL
    {
        bool addSpecilization(Specialization specilization);
        bool deleteSpecilization(Specialization specilization);
        bool updateSpecilization(Specialization specilization);

        bool addEmployee(Employee employee);
        bool deleteEmployee(Employee employee);
        bool updateEmployee(Employee employee);

        bool addContract(Contract contract);
        bool deleteContract(Contract contract);
        uint getNextContractID();

        bool addEmployer(Employer employer);
        bool deleteEmployer(Employer employer);
        bool updateEmployer(Employer employer);

        List<Specialization>    getSpecilizationList();
        List<Employee>          getEmployeeList();
        List<Employer>          getEmployerList();
        List<Contract>          getContractList();
        List<Bank>              getBankList();
    }
}
