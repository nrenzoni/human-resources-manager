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
        static string prefix = @"../../../"; // relative source of files

        static public string specName     = "specializations";
        static public string bankName     = "banks";
        static public string contractName = "contracts";
        static public string employeeName = "employees";
        static public string employerName = "employers";

        static XML_Source()
        {
            // initialize roots

            loadOrCreate(specName, specializationRoot);
            loadOrCreate(bankName, bankRoot);
            loadOrCreate(contractName, contractRoot);
            loadOrCreate(employerName, employerRoot);
            loadOrCreate(employeeName, employeeRoot);
        }

        static public XElement specializationRoot;
        static public XElement bankRoot;
        static public XElement contractRoot;
        static public XElement employeeRoot;
        static public XElement employerRoot;

        static void loadOrCreate(string filename, XElement root)
        {
            if (!File.Exists(concatXMLName(filename)))
                createXMLFile(filename, root);
            else
                loadXMLFile(filename, root);
        }

        static public string concatXMLName(string filename)
            => prefix + filename + ".xml";

        static void createXMLFile(string filename, XElement root)
        {
            root = new XElement(filename);
            root.Save(concatXMLName(filename));
        }

        static void loadXMLFile(string filename, XElement root)
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


        //static void Serialize<T>(T obj)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
        //    TextWriter writer;

        //    if (typeof(T) == typeof(Specialization))
        //        writer = new StreamWriter(concatXMLName(specName));

        //    else if (typeof(T) == typeof(Employer))
        //        writer = new StreamWriter(concatXMLName(employerName));

        //    else if (typeof(T) == typeof(Employee))
        //        writer = new StreamWriter(concatXMLName(employeeName));

        //    else if (typeof(T) == typeof(Contract))
        //        writer = new StreamWriter(concatXMLName(contractName));

        //    else return;

        //    serializer.Serialize(writer, obj);
        //    writer.Close();
        //}
    }
}
