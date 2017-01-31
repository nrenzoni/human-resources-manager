using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;

namespace DAL
{
    public class DAL_XML_Imp : IDAL
    {
        bool Exists<T>(XElement XRoot, XElement element)
        {
            if (typeof(T) == typeof(Specialization))
            {
                return (from spec in XRoot.Elements()
                        where spec.Attribute("ID").Value == element.Attribute("ID").Value
                        select spec).Any();
            }

            if (typeof(T) == typeof(Bank))
            {

            }

            if (typeof(T) == typeof(Contract))
            {

            }

            if (typeof(T) == typeof(Employee))
            {

            }

            if (typeof(T) == typeof(Employer))
            {

            }

            throw new Exception("DAL_XML_Imp error: " + typeof(T) + " cannot be used as generic T");
        }
        

        public bool addSpecilization(Specialization spec)
        {
            XElement specialization =
                new XElement("specialization", new XAttribute("ID", spec.ID),
                  new XElement("specilizationName", spec.specilizationName.ToString()),
                  new XElement("maxWagePerHour", spec.maxWagePerHour),
                  new XElement("minWagePerHour", spec.minWagePerHour)
                );

            if (Exists<Specialization>(XML_Source.specializationRoot, specialization))
                throw new Exception(spec.ID + " already exists in file");

            XML_Source.specializationRoot.Add(specialization);
            XML_Source.SaveXML<Specialization>();
            return true;
        }
        public bool deleteSpecilization(Specialization specilization);
        public bool updateSpecilization(Specialization specilization);

        public bool addEmployee(Employee employee);
        public bool deleteEmployee(Employee employee);
        public bool updateEmployee(Employee employee);
         
        public bool addContract(Contract contract);
        public bool deleteContract(Contract contract);
        public bool updateContract(Contract oldContract, Contract newContract);
        public uint getNextContractID();
         
        public bool addEmployer(Employer employer);
        public bool deleteEmployer(Employer employer);
        public bool updateEmployer(Employer employer);
         
        public List<Specialization> getSpecilizationList()
        {
            try
            { 
                return (from spec in XML_Source.specializationRoot.Elements()
                            select new Specialization()
                            {
                                specilizationName = (SpecializationName)Enum.Parse(typeof(SpecializationName), 
                                                                            spec.Element("specilizationName").Value),
                                maxWagePerHour = double.Parse(spec.Element("maxWagePerHour").Value),
                                minWagePerHour = double.Parse(spec.Element("minWagePerHour").Value)
                            }).ToList();
            }
            catch
            {
                throw new Exception("getSpecilizationList() exception");
            }
        }
        public List<Employee> getEmployeeList();
        public List<Employer> getEmployerList();
        public List<Contract> getContractList();
    }
}
