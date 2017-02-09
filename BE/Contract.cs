using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract : INotifyPropertyChanged
    {
        uint _contractID;
        public uint contractID
        {
            get { return _contractID; }
            set
            {
                if(_contractID != value)
                {
                    _contractID = value;
                    NotifyPropertyChanged("contractID");
                }
            }
        }

        uint _EmployerID;
        public uint EmployerID
        {
            get { return _EmployerID; }
            set
            {
                if(_EmployerID != value)
                {
                    _EmployerID = value;
                    NotifyPropertyChanged("EmployerID");
                }
            }
        }

        uint _EmployeeID;
        public uint EmployeeID
        {
            get { return _EmployeeID; }
            set
            {
                if (_EmployeeID != value)
                {
                    _EmployeeID = value;
                    NotifyPropertyChanged("EmployeeID");
                }
            }
        }

        bool _isInterviewed;
        public bool isInterviewed
        {
            get { return _isInterviewed; }
            set
            {
                if(_isInterviewed != value)
                {
                    _isInterviewed = value;
                    NotifyPropertyChanged("isInterviewed");
                }
            }
        }

        bool _contractFinalized = false; // default to false
        public bool contractFinalized  // contract was signed by both parties
        {
            get { return _contractFinalized; }
            set
            {
                if (_contractFinalized != value)
                {
                    _contractFinalized = value;
                    NotifyPropertyChanged("contractFinalized");
                }
            }
        }

        double _grossWagePerHour;
        public double grossWagePerHour // before taxes etc
        {
            get { return _grossWagePerHour; }
            set
            {
                if (_grossWagePerHour != value)
                {
                    _grossWagePerHour = value;
                    NotifyPropertyChanged("grossWagePerHour");
                }
            }
        }

        double _netWagePerHour;
        public double netWagePerHour // payment employee receives
        {
            get { return _netWagePerHour; }
            set
            {
                if (_netWagePerHour != value)
                {
                    _netWagePerHour = value;
                    NotifyPropertyChanged("netWagePerHour");
                }
            }
        }

        public double profit { get { return grossWagePerHour - netWagePerHour; } }

        DateTime _contractEstablishedDate = DateTime.Today;
        public DateTime contractEstablishedDate
        {
            get { return _contractEstablishedDate; }
            set
            {
                if (_contractEstablishedDate != value)
                {
                    _contractEstablishedDate = value;
                    NotifyPropertyChanged("contractEstablishedDate");
                }
            }
        }

        DateTime _contractTerminatedDate = DateTime.Today.AddYears(3);
        public DateTime contractTerminatedDate
        {
            get { return _contractTerminatedDate; }
            set
            {
                if (_contractTerminatedDate != value)
                {
                    _contractTerminatedDate = value;
                    NotifyPropertyChanged("contractTerminatedDate");
                }
            }
        }

        uint _maxWorkHours;
        public uint maxWorkHours  // per week
        {
            get { return _maxWorkHours; }
            set
            {
                if (_maxWorkHours != value)
                {
                    _maxWorkHours = value;
                    NotifyPropertyChanged("maxWorkHours");
                }
            }
        }

        public override string ToString()
            => "ID: " + contractID + ", Employer ID: " + EmployerID + ", Employee ID: " + EmployeeID + " " +
            (isInterviewed ? "was" : "was not") + " interviewed" +
            ", contract " + (contractFinalized ? "is finalized" : "is not finalized") +
            ", gross wage per hour: " + grossWagePerHour + ", net wage per hour: " + netWagePerHour +
            ", contract established date: " + contractEstablishedDate + ", contract " +
            ((DateTime.Today.Date - contractTerminatedDate.Date).Days > 0 ? "terminated" : "terminates") + " on: "
            + contractTerminatedDate.ToShortDateString() +
            ", max work hours: " + maxWorkHours;


        //public static bool operator ==(Contract c1, Contract c2)
        //{
        //    return c1.contractID == c2.contractID;
        //}

        public override bool Equals(object other)
         => contractID == (other as Contract)?.contractID;

        //public static bool operator !=(Contract c1, Contract c2)
        //{
        //    return c1.contractID != c2.contractID;
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
