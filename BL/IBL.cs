using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
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

        IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition = null);
        int ContractListByFilterCount(Predicate<Contract> condition = null);

        IEnumerable<IGrouping<Specialization, Contract>> groupContractBySpec(bool ordered = false);
        IEnumerable<IGrouping<string, Contract>> groupContractByEmployerCity(bool ordered = false);
        IEnumerable<IGrouping<string, Contract>> groupContractByEmployeeCity(bool ordered = false);
        IEnumerable<IGrouping<int, double>> getProfitByYear(bool ordered = false); // <int=year (key), double=profit>

        List<Specialization> getSpecilizationList();
        List<Employee> getEmployeeList();
        List<Employer> getEmployerList();
        List<Contract> getContractList();
    }
}
