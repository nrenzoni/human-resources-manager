using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class List_Source
    {
        public static List<Employee> employeeList = new List<Employee>();
        public static List<Employer> employerList = new List<Employer>();
        public static List<Specialization> specList = new List<Specialization>();
        public static List<Contract> contractList = new List<Contract>();

        public static List<Bank> bankList = new List<Bank>();

        static List_Source()
        {
            generateSpecs();
            generateBanks();
            generateEmployerEmployeeData();
            generateContracts();
        }

        private static void generateSpecs()
        {
            Random randGen = new Random();

            string[] specNames = { "programming", "graphic design", "management", "Communications" };
            uint i = 0;
            foreach (string spec in specNames)
            {
                specList.Add(new Specialization
                {
                    ID = i++,
                    specializationName = spec,
                    minWagePerHour = randGen.Next(50, 250),
                    maxWagePerHour = randGen.Next(250, 1000)
                });
            }
        }

        private static void generateBanks()
        {
            List<Bank> tempBanks = new List<Bank> { 
                new Bank("Leumi", 1155, 19, new CivicAddress() {Address="20 Havaad Haleumi St", City="Jerusalem" }),
                new Bank("Hapoalim", 9866, 24, new CivicAddress() {Address="14 Malki St", City="Jerusalem" }),
                new Bank("Discount", 8543, 11, new CivicAddress() { Address="Hagai St", City="Jerusalem" }),
                new Bank("Bank of Jerusalem", 6654, 8, new CivicAddress() { Address="Keren Hayesod St", City="Jerusalem" })
            };

            foreach (Bank bank in tempBanks)
            {
                bankList.Add(bank);
            }
        }

        private static void generateEmployerEmployeeData()
        {
            Random randGen = new Random();

            int[] randIDs = (from num in Enumerable.Range(1, 20)
                             select randGen.Next(10000000, 100000000)).ToArray();

            string[] firstNames = { "Terresa", "Darrick", "Lue", "Phillis", "Haywood", "Shari", "Ginette", "Connie", "Demetrius", "Priscila", "Brittani", "Olimpia", "Luanne", "Brunilda", "Nevada", "Charmain", "Boyd", "Krysta", "Winifred", "Vonnie" };

            string[] lastNames = { "Huie", "Dilbeck", "Morrow", "Millay", "Nastasi", "Spindler", "Leaf", "Bullen", "Ollis", "Satterwhite", "Spinelli", "Berney", "Skeen", "Wenrich", "Bergin", "Kummer", "Torres", "Kruger", "Burtch", "Knutson" };

            string[] compNames = { "3Com Corp", "3M Company", "A.G. Edwards Inc.", "Abbott Laboratories", "Abercrombie & Fitch Co.", "ABM Industries Incorporated", "Ace Hardware Corporation", "ACT Manufacturing Inc.", "Acterna Corp.", "Adams Resources & Energy, Inc.", "ADC Telecommunications, Inc.", "Adelphia Communications Corporation" };

            DateTime[] dates = (from offset in Enumerable.Range(1, 20)
                                select new DateTime(1960, 1, 1).AddYears(randGen.Next(40)).AddMonths(randGen.Next(12)).AddDays(randGen.Next(30))).ToArray();

            string[] phoneNums = (from i in Enumerable.Range(1, 20)
                               let num = "05" + randGen.Next(10000000, 99999999)
                               select num).ToArray();

            int[] yearsxp = (from i in Enumerable.Range(1, 20)
                             select randGen.Next(1, 40)).ToArray();

            string[] emails = { "jgranos@hotmail.com", "sparkzilla@cableone.net", "lowkell@gmail.com", "mcgregor@uwo.ca", "tamtruong99@yahoo.com", "eve@thecsrgroup.com", "cyber_zac52@hotmail.com", "clarkfa@2mawnr.usmc.mil", "hsa@uzsi.cz", "tjnichols@fsbdial.co.uk", "daniel.hiestand@3-a.ch", "dale_turner@scotiacapital.com", "Sinister13thUrge@aol.com", "gary_san@yahoo.com", "stehlyja@1mawmag12.usmc.mil", "racorrea@mre.gov.br", "kangrc@gmail.com", "outremere@comcast.net", "yvonne.deboer@international.gc.ca", "jbloore@aol.com", "arif@alfalahsec.com", "milan@eim.ae", "acbortree@yahoo.com", "dm_heilig@yahoo.com", "lazy7777@aol.com", "edp7@email.byu.edu" };

            int[] bankAccnts = Enumerable.Range(1000, 20).ToArray();
            
            string[] cities = { "אופקים","אור יהודה","אור עקיבא","אילת","אריאל","אשדוד","אשקלון","באר שבע","בית שאן","בית שמש","ביתר עילית","בני ברק","בת ים","גבעתיים","דימונה","הוד השרון","הרצליה","חדרה","חולון","חיפה","טבריה","טירת כרמל","יבנה","יהוד - מונוסון","ירושלים","כפר סבא","כרמיאל","לוד","מגדל העמק","מודיעין - מכבים - רעות","מעלה אדומים","מעלות - תרשיחא","נס ציונה","נצרת","נצרת עילית","נשר","נתיבות","נתניה","עכו","עפולה","ערד","פתח תקוה","צפת","קרית אונו","קרית אתא","קרית ביאליק","קרית גת","קרית ים","קרית מוצקין","קרית מלאכי","קרית שמונה","ראש העין","ראשון לציון","רחובות","רמלה","רמת גן","רמת השרון","רעננה","שדרות","תל אביב - יפו" };

            foreach (uint i in Enumerable.Range(0,10))
            {
                employeeList.Add(
                    new Employee
                    {
                        address = new CivicAddress { City = cities[randGen.Next(cities.Length)] },
                        ID = (uint)randIDs[i],
                        firstName = firstNames[i],
                        lastName = lastNames[i],
                        birthday = dates[i],
                        isMale= (i % 2 == 0 ? true : false),
                        email = emails[i],
                        bankAccountNumber = (uint)bankAccnts[i],
                        bank = bankList[randGen.Next(bankList.Count)],
                        phoneNumber = phoneNums[i],
                        yearsOfExperience = (uint)yearsxp[i],
                        armyGraduate = (i%2 == 0 ? true : false),
                        education = (Education)randGen.Next(1, 4),
                        specializationID = specList[randGen.Next(specList.Count)].ID
                    });

                employerList.Add(
                    new Employer
                    {
                        ID = (uint)randIDs[i + 10],
                        firstName = firstNames[i + 10],
                        lastName = lastNames[i + 10],
                        address = new CivicAddress { City = cities[randGen.Next(cities.Length)] },
                        phoneNumber = phoneNums[i + 10],
                        privatePerson = false,
                        companyName = compNames[i],
                        establishmentDate = dates[i + 10],
                        specializationID = i
                    });
            }
        }

        private static void generateContracts()
        {
            Random randGen = new Random();

            int[] sequencialIDs = (from num in Enumerable.Range(10000000, 10)
                             select num).ToArray();

            DateTime[] ContractStartDates = (from offset in Enumerable.Range(1, 10)
                                             select new DateTime(1980, 1, 1).AddYears(randGen.Next(20)).AddMonths(randGen.Next(12)).AddDays(randGen.Next(30))).ToArray();

            DateTime[] ContractTermDates = (from offset in Enumerable.Range(1, 10)
                                            select new DateTime(2010, 1, 1).AddYears(randGen.Next(20)).AddMonths(randGen.Next(12)).AddDays(randGen.Next(30))).ToArray();

            for (int i=0; i<10; i++)
            {
                var tempGrossWage = (specList.Find(x => x.ID == employeeList[i].specializationID).minWagePerHour + specList.Find(x => x.ID == employeeList[i].specializationID).maxWagePerHour) / 2;
                contractList.Add(new Contract
                {
                    contractEstablishedDate = ContractStartDates[i],
                    contractTerminatedDate = ContractTermDates[i],
                    contractID = (uint)sequencialIDs[i],
                    contractFinalized = (randGen.Next(0, 1) == 0 ? true : false),
                    isInterviewed = (randGen.Next(0, 1) == 0 ? true : false),
                    EmployeeID = employeeList[i].ID,
                    EmployerID = employerList[i].ID,
                    maxWorkHours = (uint)randGen.Next(25, 50),
                    grossWagePerHour = tempGrossWage,
                    netWagePerHour = tempGrossWage - randGen.NextDouble() * .1 * tempGrossWage
                });
            }
        }
    }
}
