using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employer : INotifyPropertyChanged
    {
        bool _privatePerson;
        public bool privatePerson
        {
            get { return _privatePerson; }
            set
            {
                if(_privatePerson != value)
                {
                    _privatePerson = value;
                    NotifyPropertyChanged();
                }
            }
        }

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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        string _phoneNumber;
        public string phoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        DateTime _establishmentDate = new DateTime(2000,1,1);
        public DateTime establishmentDate
        {
            get { return _establishmentDate; }
            set
            {
                if (_establishmentDate != value)
                {
                    _establishmentDate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public override string ToString()
            => ID + ": " + companyName;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
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
