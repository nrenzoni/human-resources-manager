using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BL
{
    public interface IBL
    {
        bool addSpecialization(Specialization specilization);

        /// <summary>
        /// deletes specialization on condition that not in use by employee or employer
        /// </summary>
        /// <param name="specilization"></param>
        /// <returns></returns>
        bool deleteSpecilization(Specialization specilization);

        /// <summary>
        /// unconditionaly update any property of specialization, except for ID
        /// </summary>
        /// <param name="specilization"></param>
        /// <returns></returns>
        bool updateSpecilization(Specialization specilization);

        /// <summary>
        /// add employee on condition that not below 18 years old, and check that his bank account does not exist for someone else
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        bool addEmployee(Employee employee);

        /// <summary>
        /// deletes employee only if no open contracts
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// 
        bool deleteEmployee(Employee employee);

        /// <summary>
        /// updates contract without any BL checks
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        bool updateEmployee(Employee employee);

        /// <summary>
        /// add contracts on condition that employer and employee already exist, company established more than a year ago.
        /// in addition, net wage is calculated.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        bool addContract(Contract contract);

        /// <summary>
        /// deletes contract on condition that contract termination date has passed. 
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        bool deleteContract(Contract contract);

        /// <summary>
        /// contract can only be updated if contract termination date has passed. 
        /// </summary>
        /// <param name="oldContract"></param>
        /// <param name="newContract"></param>
        /// <returns></returns>
        bool terminateContract(Contract contract);

        /// <summary>
        /// returns next available contract ID from DAL
        /// </summary>
        /// <returns></returns>
        uint getNextContractID();

        /// <summary>
        /// returns next available specialization ID from DAL
        /// </summary>
        /// <returns></returns>
        uint getNextSpecID();

        /// <summary>
        /// adds employer on condition that company establishment is not in future, and that same company does not already exist
        /// </summary>
        /// <param name="employer"></param>
        /// <returns></returns>
        bool addEmployer(Employer employer);

        /// <summary>
        /// delete employer on condition that he does not have any open contracts
        /// </summary>
        /// <param name="employer"></param>
        /// <returns></returns>
        bool deleteEmployer(Employer employer);

        /// <summary>
        /// unconditionally updates employer
        /// </summary>
        /// <param name="employer"></param>
        /// <returns></returns>
        bool updateEmployer(Employer employer);


        /// <summary>
        /// returns contracts by lambda expression
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition = null);

        /// <summary>
        /// overloaded method that returns returns contracts by lambda expression as well as count of filtered contracts as well
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<Contract> getContractListByFilter(Predicate<Contract> condition, out int count);


        /// <summary>
        ///     returns contracts grouped by employee specializations.
        ///     optional lexicographical sorting of groups by specialization name
        /// </summary>
        /// <param name="ordered"></param>
        /// <returns></returns>
        IEnumerable<ContractGroupingContainer> groupContractByEmployeeSpec(bool ordered = false);

        /// <summary>
        /// returns contracts grouped by employer city.
        ///  optional lexicographical sorting of groups by employer city, and within each group sorting by address
        /// </summary>
        /// <param name="ordered"></param>
        /// <returns></returns>
        IEnumerable<ContractGroupingContainer> groupContractByEmployerCity(bool ordered = false);

        /// <summary>
        /// returns contracts grouped by employee city.
        ///  optional lexicographical sorting of groups by employee city, and within each group sorting by address
        /// </summary>
        /// <param name="ordered"></param>
        /// <returns></returns>
        IEnumerable<ContractGroupingContainer> groupContractByEmployeeCity(bool ordered = false);

        /// <summary>
        /// returns profit by year of management company. key= year, value= profit
        /// </summary>
        /// <param name="ordered"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, double>> getProfitByYear(bool ordered = false); // <int=year (key), double=profit>

        /// <summary>
        /// returns banks with grouping of bank names, inner list of branches
        /// </summary>
        /// <returns></returns>
        //IEnumerable<IGrouping<string, Bank>> getBanksGrouped();

        IEnumerable<IGrouping<string, Bank>> getBanksGrouped();

        List<Specialization> getSpecilizationList();
        List<Employee> getEmployeeList();
        List<Employer> getEmployerList();
        List<Contract> getContractList();
        List<Bank> getBankList();

        IEnumerable<ContractGroupingContainer> getContractsInContainer();

        DoWorkDelegate getXMLBankBackground_DoWork();
    }
}
