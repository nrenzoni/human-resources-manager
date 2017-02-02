using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employer : INotifyPropertyChanged
    {
        public bool privatePerson { get; set; }

        public uint ID { get; set; } // ID does not change

        // if private person
        string _firstName;
        public string firstName {
            get { return _firstName; }
            set
            {
                if(_firstName != value)
                {
                    _firstName = value;
                    NotifyPropertyChanged("firstName");
                }
            }
        }

        // if private person
        string _lastName;
        public string lastName{
            get {return _lastName; }
            set
            {
                if(_lastName != value)
                {
                    _lastName = value;
                    NotifyPropertyChanged("lastName");
                }
            }
        }

        string _companyName;
        public string companyName
        {
            get { return _companyName; }
            set
            {
                if(_companyName != value)
                {
                    _companyName = value;
                    NotifyPropertyChanged("companyName");
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

        uint _specializationID;  // enum
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

        DateTime _establishmentDate;
        public DateTime establishmentDate
        {
            get { return _establishmentDate; }
            set
            {
                if (_establishmentDate != value)
                {
                    _establishmentDate = value;
                    NotifyPropertyChanged("establishmentDate");
                }
            }
        }

        public override string ToString()
        {
            return ((privatePerson) ? "Private Employer" : "Company") +
                " ID: " + ID +
                ((privatePerson) ? " Name: " +
                lastName + ", " + firstName : "") +
                ((string.IsNullOrEmpty(companyName)) ? "" : " Company Name: " + companyName) +
                " Phone Number: " + phoneNumber +
                ", " + address;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public override bool Equals(object other)
            => ID == (other as Employer)?.ID;

        public static bool operator ==(Employer e1, Employer e2)
        {
            return e1?.ID == e2?.ID;
        }

        public static bool operator !=(Employer e1, Employer e2)
        {
            return e1?.ID != e2?.ID;
        }
    }
}
