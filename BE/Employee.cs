using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BE
{
    public enum Education { student, BA, MA, PHD};

    public class CivicAddress
    {
        
        public string Address { get; set; }
        public string City { get; set; }
        public override string ToString()
            => (!string.IsNullOrEmpty(Address) ?  "Address: " + Address + ", " : "" ) + "City: " + City;

        static public List<string> Cities = new List<string> { "עפולה", "עכו", "ערד", "אריאל", "אשדוד", "אשקלון", "בת ים", "באר שבע", "בית שאן", "בית שמש", "ביתר עילית", "בני ברק", "דימונה", "אילת", "גבעתיים", "חדרה", "חיפה", "הרצליה", "הוד השרון", "חולון", "ירושלים", "כרמיאל", "כפר סבא", "קרית אתא", "קרית ביאליק", "קרית גת", "קרית מלאכי", "קרית מוצקין", "קרית אונו", "קרית שמונה", "קרית ים", "לוד", "מעלה אדומים", "מעלות-תרשיחא", "מגדל העמק", "מודיעין-מכבים-רעות", "נצרת", "נצרת עילית", "נס ציונה", "נשר", "נתניה", "נתיבות", "אופקים", "אור עקיבא", "אור יהודה", "פתח תקוה", "רעננה", "רמת השרון", "רמת גן", "רמלה", "רחובות", "ראשון לציון", "ראש העין", "שדרות", "תל אביב-יפו", "טבריה", "טירת כרמל", "צפת", "יבנה", "יהוד-מונוסון" };
    }

    public class Employee :INotifyPropertyChanged
    {
        // no recommendation in constructor; recommendation assigned using property
        //public Employee(uint _ID, string _firstName, string _lastName, DateTime _birthday, uint _phone, CivicAddress _address, uint _yearsOfExperience, string _email, Education _education, bool _armyGraduate, Bank _bank, uint _bankAccountNumber)
        //{
        //    ID = _ID;
        //    lastName = _lastName;
        //    firstName = _firstName;
        //    birthday = _birthday;
        //    phoneNumber = _phone;
        //    address = _address;
        //    yearsOfExperience = _yearsOfExperience;
        //    email = _email;
        //    education = _education;
        //    armyGraduate = _armyGraduate;
        //    bank = _bank;
        //    bankAccountNumber = _bankAccountNumber;
        //}

        public uint ID { get; set; }

        string _lastName;
        public string lastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName!=value)
                {
                    _lastName = value;
                    NotifyPropertyChanged("lastName");
                    
                }
            }
        }

        string _firstName;
        public string firstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    NotifyPropertyChanged("firstName");
                }
            }
        }

        bool _isMale;
        public bool isMale
        {
            get { return _isMale; }
            set
            {
                if(_isMale!=value)
                {
                    _isMale = value;
                    NotifyPropertyChanged("isMale");
                }
            }
        }

        DateTime _birthday;
        public DateTime birthday
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    NotifyPropertyChanged("birthday");
                }
            }
        }

        uint _phoneNumber;
        public uint phoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    NotifyPropertyChanged("phoneNumber");
                }
            }
        }

        CivicAddress _address;
        public CivicAddress address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    NotifyPropertyChanged("address");
                }
            }
        }

        uint _yearsOfExperience;
        public uint yearsOfExperience
        {
            get { return _yearsOfExperience; }
            set
            {
                if (_yearsOfExperience != value)
                {
                    _yearsOfExperience = value;
                    NotifyPropertyChanged("yearsOfExperience");
                }
            }
        }

        string _recommendationNotes;
        public string recommendationNotes
        {
            get { return _recommendationNotes; }
            set
            {
                if (_recommendationNotes != value)
                {
                    _recommendationNotes = value;
                    NotifyPropertyChanged("recommendationNotes");
                }
            }
        }

        string _email;
        public string email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    NotifyPropertyChanged("email");
                }
            }
        }

        Education _education;
        public Education education
        {
            get { return _education; }
            set
            {
                if (_education != value)
                {
                    _education = value;
                    NotifyPropertyChanged("education");
                }
            }
        }

        bool _armyGraduate;
        public bool armyGraduate
        {
            get { return _armyGraduate; }
            set
            {
                if (_armyGraduate != value)
                {
                    _armyGraduate = value;
                    NotifyPropertyChanged("armyGraduate");
                }
            }
        }

        Bank _bank;
        public Bank bank
        {
            get { return _bank; }
            set
            {
                if (_bank != value)
                {
                    _bank = value;
                    NotifyPropertyChanged("bank");
                }
            }
        }

        uint _bankAccountNumber;
        public uint bankAccountNumber
        {
            get { return _bankAccountNumber; }
            set
            {
                if (_bankAccountNumber != value)
                {
                    _bankAccountNumber = value;
                    NotifyPropertyChanged("bankAccountNumber");
                }
            }
        }

        uint _specializationID;
        public uint specializationID
        {
            get { return _specializationID; }
            set
            {
                if (_specializationID != value)
                {
                    _specializationID = value;
                    NotifyPropertyChanged("specializationID");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }





        public override string ToString()
        => _lastName + ", " + _firstName + ". ID: " + ID +
            ", Birthday: " + _birthday.ToString("d") + ", " +
            (_isMale ? "Male" : "Female") +
            ", Phone: " +
            _phoneNumber + ", " + _address + ", Years of Experience: " +
            _yearsOfExperience + ", Education: " +
            _education.ToString() + ", " +
            (_armyGraduate ? " served in Army" : "did not serve in Army") +
            (string.IsNullOrEmpty(_recommendationNotes) ? ", recommendation Notes: " + _recommendationNotes : "") +
            ", Bank Account #: " + _bankAccountNumber;

        public bool Equals(Employee emp)
            => ID == emp.ID;
        public static bool operator ==(Employee e1, Employee e2)
        {
            return e1?.specializationID == e2?.specializationID;
        }

        public static bool operator !=(Employee e1, Employee e2)
        {
            return e1?.specializationID != e2?.specializationID;
        }
    }
}
