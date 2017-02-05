using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;

namespace DS
{
    static public class XML_Source
    {
        static string prefix = @"../../../XML DB Files/"; // relative source of files

        static public string specName     = "specializations";
        static public string bankName     = "banks";
        static public string contractName = "contracts";
        static public string employeeName = "employees";
        static public string employerName = "employers";

        static public XElement specializationRoot;
        static public XElement bankRoot;
        static public XElement contractRoot;
        static public XElement employeeRoot;
        static public XElement employerRoot;

        static public IEnumerable<Bank> Banks;

        static XML_Source()
            {
            // initialize roots

            loadOrCreate(specName, out specializationRoot);
            loadOrCreate(contractName, out contractRoot);
            loadOrCreate(employerName, out employerRoot);
            loadOrCreate(employeeName, out employeeRoot);



            // download bank.xml, and load into list

            loadXMLFile(bankName, out bankRoot);
            Banks = from bank in bankRoot.Elements()
                    select new Bank()
                    {
                        BankName = (string)bank.Element("BankName"),
                        BankNumber = (uint)bank.Element("Branch"),
                        Branch = (uint)bank.Element("Branch"),
                        Address = (CivicAddress)bank.Element("CivicAddress"),
                    };

            //try
            //{
            //    XElement banks = XElement.Load(@"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml");

            //    Banks = from XBank in banks.Elements()
            //             let tempAddress = new CivicAddress
            //             {
            //                 Address = (string)XBank.Element("כתובת_ה-ATM"),
            //                 City = (string)XBank.Element("ישוב")
            //             }
            //             group new Bank
            //             {
            //                 BankName = (string)XBank.Element("שם_בנק"),
            //                 BankNumber = (uint)XBank.Element("קוד_בנק"),
            //                 Address = tempAddress,
            //                 Branch = (uint)XBank.Element("קוד_סניף")
            //             } by (string)XBank.Element("קוד_בנק") + (string)XBank.Element("כתובת_ה-ATM") into bankNumAndAddress
            //             select bankNumAndAddress.First();

            //    // saves banks to banks.xml
            //    XmlSerializer serializer = new XmlSerializer(typeof(List<Bank>));
            //    TextWriter writer = new StreamWriter((concatXMLName("banks")));
            //    serializer.Serialize(writer, Banks.ToList());
            //    writer.Close();

            //    // loads saves banks.xml file into bankRoot
            //    loadXMLFile(bankName, out bankRoot);
            //}
            //catch // if internet problem, enter into catch
            //{
            //    // banks.xml should be on local hard drive
            //    loadXMLFile(bankName, out bankRoot);
            //    Banks = from bank in bankRoot.Elements()
            //            select new Bank()
            //            {
            //                BankName = (string)bank.Element("BankName"),
            //                BankNumber = (uint)bank.Element("Branch"),
            //                Branch = (uint)bank.Element("Branch"),
            //                Address = (CivicAddress)bank.Element("Address"),
            //            };
            //}

            if (bankRoot == null)
                throw new Exception("banks not initalized");

        }

        static void loadOrCreate(string filename, out XElement root)
        {
            if (!File.Exists(concatXMLName(filename)))
                createXMLFile(filename, out root);
            else
                loadXMLFile(filename, out root);
        }

        static public string concatXMLName(string filename)
            => prefix + filename + ".xml";

        static void createXMLFile(string filename, out XElement root)
        {
            root = new XElement(filename);
            root.Save(concatXMLName(filename));
        }

        static void loadXMLFile(string filename, out XElement root)
        {
            try
            {
                root = XElement.Load(concatXMLName(filename));
            }
            catch
            {
                throw new Exception("error loading " + concatXMLName(filename));
            }
        }

        static public void SaveXML<T>()
        {
            if (typeof(T) == typeof(Specialization))
                specializationRoot.Save(concatXMLName(specName));

            else if (typeof(T) == typeof(Bank))
                bankRoot.Save(concatXMLName(bankName));

            else if (typeof(T) == typeof(Contract))
                contractRoot.Save(concatXMLName(contractName));

            else if (typeof(T) == typeof(Employee))
                employeeRoot.Save(concatXMLName(employeeName));

            else if (typeof(T) == typeof(Employer))
                employerRoot.Save(concatXMLName(employerName));

            else
                throw new Exception("bad type passed to T of SaveXML<T>()");
        }
    }
}
