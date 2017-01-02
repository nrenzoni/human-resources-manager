using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    enum SpecializationName { Programming, Communications, SoftwareSecurity, GraphicDesign };

    public class Specialization
    {
        /// <summary>
        /// second parameter is int[]; first index is minimum wage, second index is maximum wage per hour, and third index specializationID
        /// </summary>
        static Dictionary<string, int[]> fieldNames = new Dictionary<string, int[]>()
        {
            { "Programming",        new int[] { 8,10,0 } },
            {"Communications",      new int[] { 6,10,1} },
            {"SoftwareSecurity",    new int[] { 7,9,2} },
            { "GraphicDesign",      new int[] { 4,6,3} },
            { "QA",                 new int[] { 1,5,4} }
        };

        int _specializationID;
        public int SpecializationID { get { return _specializationID; } }

        int minWage, maxWage;
        string _SpecializationField;

        public string SpecializationField
        {
            get
            {
                return _SpecializationField;
            }

            set // value should be key in fieldNames OrderedDict
            {
                if (!fieldNames.ContainsKey(value))
                    throw new Exception("specialty field does not exist in dictionary");

                _SpecializationField = value;

                // assigns first and second value at respective index of int[] key in fieldNames OrderedDictionary
                minWage = fieldNames[value][0];
                maxWage = fieldNames[value][1];

                _specializationID = fieldNames[value][2];
            }
        }

        public override string ToString()
        {
            return "Specialty: " + SpecializationField + " min wage: " + minWage + ", max wage: " + maxWage + "\n";
        }
    }
}
///