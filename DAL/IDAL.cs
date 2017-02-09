using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.ComponentModel;

namespace DAL
{
    public interface IDAL
    {
        bool addSpecilization(Specialization specilization);
        bool deleteSpecilization(Specialization specilization);
        bool updateSpecilization(Specialization specilization);
        uint getNextSpecID();

        bool addEmployee(Employee employee);
        bool deleteEmployee(Employee employee);
        bool updateEmployee(Employee employee);

        bool addContract(Contract contract, bool autoAssignID= true);
        bool deleteContract(Contract contract);
        bool updateContract(Contract contract);
        uint getNextContractID();

        bool addEmployer(Employer employer);
        bool deleteEmployer(Employer employer);
        bool updateEmployer(Employer employer);

        List<Specialization>    getSpecilizationList();
        List<Employee>          getEmployeeList();
        List<Employer>          getEmployerList();
        List<Contract>          getContractList();
        List<Bank>              getBankList();

        /// <summary>
        /// returns BackgroundWorker delegate that downloads/loads Bank XML file into DB.
        /// </summary>
        /// <returns></returns>
        DoWorkDelegate getXMLBankBackground_DoWork();
    }
}
