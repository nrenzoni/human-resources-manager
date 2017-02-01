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
        // check if element already exists in XML file
        XElement ElementIfExists(XElement root, XElement element)
            => (from spec in root.Elements()
                    where spec.Attribute("ID").Value == element.Attribute("ID").Value
                    select spec).FirstOrDefault();

        // overloaded method finds element based on ID
        XElement ElementIfExists(XElement root, uint ID)
            => (from spec in root.Elements()
                where uint.Parse(spec.Attribute("ID").Value) == ID
                select spec).FirstOrDefault();

        bool removeElementFromXML(XElement XRoot, XElement element)
        {
            XElement foundElement = ElementIfExists(XRoot, element);
            if (foundElement != null) // element found
            {
                foundElement.Remove();
                XML_Source.SaveXML<Specialization>();
                return true;
            }
            else
                throw new Exception("element " + element.Attribute("ID") + " not found in XML");
        }

        bool removeElementFromXML(XElement XRoot, uint ID)
        {
            XElement foundElement = ElementIfExists(XRoot, ID);
            if (foundElement != null) // element found
            {
                foundElement.Remove();
                XML_Source.SaveXML<Specialization>();
                return true;
            }
            else
                throw new Exception("element " + ID + " not found in XML");
        }

        XElement createSpecXElement(Specialization spec)
            => new XElement("specialization", new XAttribute("ID", spec.ID),
                  new XElement("specializationName", spec.specializationName),
                  new XElement("maxWagePerHour", spec.maxWagePerHour),
                  new XElement("minWagePerHour", spec.minWagePerHour)
                );

        public bool addSpecilization(Specialization spec)
        {
            if (ElementIfExists(XML_Source.specializationRoot, spec.ID) != null)
                throw new Exception(spec.ID + " already exists in file");

            XML_Source.specializationRoot.Add(createSpecXElement(spec));
            XML_Source.SaveXML<Specialization>();
            return true;
        }

        public bool deleteSpecilization(Specialization spec)
            => removeElementFromXML(XML_Source.specializationRoot, spec.ID);

        public bool updateSpecilization(Specialization spec)
        {
            XElement foundElement = ElementIfExists(XML_Source.specializationRoot, spec.ID);
            if(foundElement == null)
                throw new Exception(spec.ID + " does not exist in XML");

            return 
                removeElementFromXML(XML_Source.specializationRoot, foundElement) 
                && addSpecilization(spec);
        }

        public bool addEmployee(Employee e)
        {
            XElement employee =
                new XElement("employee", new XAttribute("ID", e.ID),
                  new XElement("firstName", e.firstName),
                  new XElement("lastName", e.lastName),
                  new XElement("isMale", e.isMale),
                  new XElement("phoneNumber", e.phoneNumber),
                  new XElement("email", e.email),
                  new XElement("birthday", e.birthday),
                  new XElement("education", e.education),
                  new XElement("armyGraduate", e.armyGraduate),
                  new XElement("yearsOfExperience", e.yearsOfExperience),
                  new XElement("specializationID", e.specializationID),
                  new XElement("civicAddress",
                    new XElement("address", e.address.Address),
                    new XElement("city", e.address.City)
                  ),
                  new XElement("bank", new XElement("bankAccount", e.bankAccountNumber),
                    new XElement("bankNumber", e.bank.BankNumber),
                    new XElement("bankBranch", e.bank.Branch)
                  ),
                  new XElement("recommendationNotes", e.recommendationNotes)
                );

            if (ElementIfExists(XML_Source.employeeRoot, employee) != null)
            {
                throw new Exception(e.ID + " already exists in file");
            }

            XML_Source.employeeRoot.Add(employee);
            XML_Source.SaveXML<Employee>();
            return true;
        }
        public bool deleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
        public bool updateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
         
        public bool addContract(Contract contract)
        {
            throw new NotImplementedException();
        }
        public bool deleteContract(Contract contract)
        {
            throw new NotImplementedException();
        }
        public uint getNextContractID()
        {
            throw new NotImplementedException();
        }
         
        public bool addEmployer(Employer employer)
        {
            throw new NotImplementedException();
        }
        public bool deleteEmployer(Employer employer)
        {
            throw new NotImplementedException();
        }
        public bool updateEmployer(Employer employer)
        {
            throw new NotImplementedException();
        }
         
        public List<Specialization> getSpecilizationList()
        {
            try
            { 
                return (from spec in XML_Source.specializationRoot.Elements()
                            select new Specialization()
                            {
                                specializationName = spec.Element("specializationName").Value,
                                maxWagePerHour    = double.Parse(spec.Element("maxWagePerHour").Value),
                                minWagePerHour    = double.Parse(spec.Element("minWagePerHour").Value)
                            }).ToList();
            }
            catch
            {
                throw new Exception("getSpecilizationList() exception");
            }
        }
        public List<Employee> getEmployeeList()
        {
            throw new NotImplementedException();
        }
        public List<Employer> getEmployerList()
        {
            throw new NotImplementedException();
        }
        public List<Contract> getContractList()
        {
            throw new NotImplementedException();
        }
    }
}
