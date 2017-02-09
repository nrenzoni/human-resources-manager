using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;
using System.ComponentModel;

namespace DAL
{
    public class DAL_XML_Imp : IDAL
    {
        static uint nextContractID;
        static uint nextSpecID;

        static DAL_XML_Imp()
        {
            setNextID(XML_Source.contractRoot, out nextContractID, 100000);
            setNextID(XML_Source.specializationRoot, out nextSpecID, 100000);
        }

        static void setNextID(XElement XRoot, out uint nextParam, uint defaultNext)
        {
            if (XRoot.HasElements == false) // no children nodes
            {
                nextParam = defaultNext;
            }
            else
                nextParam = (from node in XRoot.Elements()
                            where node.Attributes("ID").Any()
                            select (uint)node.Attribute("ID")).Max() + 1;
        }

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

            nextSpecID++;
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

        public uint getNextSpecID() => nextSpecID;

        XElement createEmployeeXElement(Employee e)
            => new XElement("employee", new XAttribute("ID", e.ID),
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
                  new XElement("CivicAddress",
                    new XElement("address", e.address.Address),
                    new XElement("city", e.address.City)
                  ),
                  new XElement("bank", new XAttribute("bankAccount", e.bankAccountNumber),
                    new XElement("BankName", e.bank.BankName),
                    new XElement("bankBranch", e.bank.Branch)
                  ),
                  new XElement("recommendationNotes", e.recommendationNotes)
                );

        public bool addEmployee(Employee e)
        {
            if (ElementIfExists(XML_Source.employeeRoot, e.ID) != null)
            {
                throw new Exception(e.ID + " already exists in file");
            }

            XML_Source.employeeRoot.Add(createEmployeeXElement(e));
            XML_Source.SaveXML<Employee>();
            return true;
        }
        public bool deleteEmployee(Employee employee)
            => removeElementFromXML(XML_Source.employeeRoot, employee.ID);

        public bool updateEmployee(Employee employee)
        {
            XElement foundElement = ElementIfExists(XML_Source.employeeRoot, employee.ID);
            if (foundElement == null)
                throw new Exception(employee.ID + " does not exist in XML");

            return
                removeElementFromXML(XML_Source.employeeRoot, foundElement)
                && addEmployee(employee);
        }

        XElement createContractXElement(Contract c)
            => new XElement("contract", new XAttribute("ID", c.contractID),
                 new XElement("EmployerID", c.EmployerID),
                 new XElement("EmployeeID", c.EmployeeID),
                 new XElement("isInterviewed", c.isInterviewed),
                 new XElement("contractFinalized", c.contractFinalized),
                 new XElement("grossWagePerHour", c.grossWagePerHour),
                 new XElement("netWagePerHour", c.netWagePerHour),
                 new XElement("maxWorkHours", c.maxWorkHours),
                 new XElement("contractEstablishedDate", c.contractEstablishedDate),
                 new XElement("contractTerminatedDate", c.contractTerminatedDate)
                 );

        public bool addContract(Contract contract, bool autoAssignID=true)
        {
            if (ElementIfExists(XML_Source.contractRoot, contract.contractID) != null)
            {
                throw new Exception(contract.contractID + " already exists in file");
            }

            if (autoAssignID)
                contract.contractID = nextContractID++;

            XML_Source.contractRoot.Add(createContractXElement(contract));
            XML_Source.SaveXML<Contract>();

            return true;
        }

        public bool deleteContract(Contract contract)
            => removeElementFromXML(XML_Source.contractRoot, contract.contractID);


        public bool updateContract(Contract contract)
        {
            XElement foundElement = ElementIfExists(XML_Source.contractRoot, contract.contractID);
            if (foundElement == null)
                throw new Exception(contract.contractID + " does not exist in XML");

            return
                removeElementFromXML(XML_Source.contractRoot, foundElement)
                && addContract(contract, false); // don't assign new contract ID
        }

        public uint getNextContractID()
        {
            return nextContractID;
        }

        XElement createEmployerXElement(Employer e)
        {
            XElement employer =  new XElement("employer", new XAttribute("ID", e.ID),
                                    new XElement("companyName", e.companyName),
                                    new XElement("phoneNumber", e.phoneNumber),
                                    new XElement("privatePerson", e.privatePerson),
                                    new XElement("CivicAddress",
                                    new XElement("address", e.address.Address),
                                    new XElement("city", e.address.City)
                                    ),
                                    new XElement("specializationID", e.specializationID),
                                    new XElement("establishmentDate", e.establishmentDate)
                                );

            // if private person, add first name and last name
            if (e.privatePerson)
                employer.AddFirst(
                    new XElement("firstName", e.firstName),
                    new XElement("lastName", e.lastName)
                    );

            return employer;
        }

        public bool addEmployer(Employer e)
        {
            if (ElementIfExists(XML_Source.employerRoot, e.ID) != null)
            {
                throw new Exception(e.ID + " already exists in file");
            }

            XML_Source.employerRoot.Add(createEmployerXElement(e));
            XML_Source.SaveXML<Employer>();
            return true;
        }

        public bool deleteEmployer(Employer employer)
            => removeElementFromXML(XML_Source.employerRoot, employer.ID);

        public bool updateEmployer(Employer employer)
        {
            XElement foundElement = ElementIfExists(XML_Source.employerRoot, employer.ID);
            if (foundElement == null)
                throw new Exception(employer.ID + " does not exist in XML");

            return
                removeElementFromXML(XML_Source.employerRoot, foundElement)
                && addEmployer(employer);
        }
         
        public List<Specialization> getSpecilizationList()
        {
            try
            { 
                return (from spec in XML_Source.specializationRoot.Elements()
                            select new Specialization()
                            {
                                ID =                 (uint)spec.Attribute("ID"),
                                specializationName = spec.Element("specializationName").Value,
                                maxWagePerHour    =  double.Parse(spec.Element("maxWagePerHour").Value),
                                minWagePerHour    =  double.Parse(spec.Element("minWagePerHour").Value)
                            }).ToList();
            }
            catch
            {
                throw new Exception("getSpecilizationList() exception");
            }
        }

        public List<Employee> getEmployeeList()
        {
            try
            {
                return (from e in XML_Source.employeeRoot.Elements()
                        let currentBank = new Bank()
                        {
                            BankName = (string)e.Element("bank").Element("BankName"),
                            Branch =(uint)e.Element("bank").Element("bankBranch")
                        }
                        select new Employee()
                        {
                            ID =                  (uint)e.Attribute("ID"),
                            firstName =           e.Element("firstName")?.Value,
                            lastName =            e.Element("lastName")?.Value,
                            address =             (CivicAddress)e.Element("CivicAddress"),
                            isMale =              (bool)e.Element("isMale"),
                            email =               e.Element("email")?.Value,
                            phoneNumber =         (string)e.Element("phoneNumber"),
                            armyGraduate =        (bool)e.Element("armyGraduate"),
                            yearsOfExperience =   (uint)e.Element("yearsOfExperience"),
                            specializationID =    (uint)e.Element("specializationID"),
                            birthday =            (DateTime)e.Element("birthday"),
                            education =           (Education)Enum.Parse(typeof(Education),e.Element("education").Value, true),
                            bankAccountNumber =   (uint)e.Element("bank").Attribute("bankAccount"),
                            bank =                currentBank,
                            recommendationNotes = (string)e.Element("recommendationNotes")
                        }).ToList();
            }
            catch
            {
                throw new Exception("getEmployeeList() exception");
            }
        }

        public List<Employer> getEmployerList()
        {
            try
            {
                return (from e in XML_Source.employerRoot.Elements()
                        select new Employer()
                        {
                            ID =                uint.Parse(e.Attribute("ID").Value),
                            companyName =       e.Element("companyName").Value,
                            privatePerson =     bool.Parse(e.Element("privatePerson").Value),
                            firstName =         (string)e.Element("firstName"), // check if exists perhaps
                            lastName =          (string)e.Element("lastName"),
                            phoneNumber =       (string)e.Element("phoneNumber"),
                            specializationID =  uint.Parse(e.Element("specializationID").Value),
                            establishmentDate = DateTime.Parse(e.Element("establishmentDate").Value),
                            address =           (CivicAddress)e.Element("CivicAddress") // calls explicit converter of Xlement to CivicAddress
                        }
                        ).ToList();
            }
            catch
            {
                throw new Exception("getEmployerList() exception");
            }
        }

        public List<Contract> getContractList()
        {
            try
            {
                return (from cont in XML_Source.contractRoot.Elements()
                        where cont.Name == "contract"
                        select new Contract()
                        {
                            contractID =              (uint)cont.Attribute("ID"),
                            EmployerID =              (uint)cont.Element("EmployerID"),
                            EmployeeID =              (uint)cont.Element("EmployeeID"),
                            isInterviewed =           (bool)cont.Element("isInterviewed"),
                            contractFinalized =       (bool)cont.Element("contractFinalized"),
                            grossWagePerHour =        (double)cont.Element("grossWagePerHour"),
                            netWagePerHour =          (double)cont.Element("netWagePerHour"),
                            contractEstablishedDate = (DateTime)cont.Element("contractEstablishedDate"),
                            contractTerminatedDate =  (DateTime)cont.Element("contractTerminatedDate"),
                            maxWorkHours =            (uint)cont.Element("maxWorkHours")
                        }).ToList();
            }
            catch { throw new Exception("getContractList() exception"); }
        }

        public List<Bank> getBankList()
        {
            var returnList = XML_Source.Banks?.ToList();

            if (returnList == null)
                return new List<Bank>();
            else
                return returnList;
        }

        public DoWorkDelegate getXMLBankBackground_DoWork()
            => XML_Source.downloadBankXml;
    }
}
