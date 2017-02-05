using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;

namespace BE
{
    public enum Education { student, BA, MA, PHD};

    public class Employee :INotifyPropertyChanged
    {
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

        bool _isMale = true; // default to male
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

        DateTime _birthday = new DateTime();
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

        CivicAddress _address = new CivicAddress();
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

        Bank _bank = new Bank();
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
            => ID + ": " + lastName + ", " + firstName;

        //public override string ToString()
        //=> _lastName + ", " + _firstName + ". ID: " + ID +
        //    ", Birthday: " + _birthday.ToString("d") + ", " +
        //    (_isMale ? "Male" : "Female") +
        //    ", Phone: " +
        //    _phoneNumber + ", " + _address + ", Years of Experience: " +
        //    _yearsOfExperience + ", Education: " +
        //    _education.ToString() + ", " +
        //    (_armyGraduate ? " served in Army" : "did not serve in Army") +
        //    (string.IsNullOrEmpty(_recommendationNotes) ? ", recommendation Notes: " + _recommendationNotes : "") +
        //    ", Bank Account #: " + _bankAccountNumber;

        public override bool Equals(object other)
            => ID == (other as Employee)?.ID;

        public static bool operator ==(Employee e1, Employee e2)
        {
            return e1?.ID == e2?.ID;
        }

        public static bool operator !=(Employee e1, Employee e2)
        {
            return e1?.ID != e2?.ID;
        }
    }
}
