using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    //public enum SpecializationName { Programming, Communications, Software_Security, Graphics_Design };

    public class Specialization : INotifyPropertyChanged
    {
        uint _ID;
        public uint ID
        {
            get { return _ID; }
            set
            {
                if(_ID != value)
                {
                    _ID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        string _specializationName;
        public string specializationName
        {
            get { return _specializationName; }
            set
            {
                if (_specializationName != value)
                {
                    _specializationName = value;
                    NotifyPropertyChanged("specializationName");
                }
            }
        }

        double _minWagePerHour;
        public double minWagePerHour
        {
            get { return _minWagePerHour; }
            set
            {
                if (_minWagePerHour != value)
                {
                    _minWagePerHour = value;
                    NotifyPropertyChanged("minWagePerHour");
                }
            }
        }

        double _maxWagePerHour;
        public double maxWagePerHour
        {
            get { return _maxWagePerHour; }
            set
            {
                if (_maxWagePerHour != value)
                {
                    _maxWagePerHour = value;
                    NotifyPropertyChanged("maxWagePerHour");
                }
            }
        }


        public override bool Equals(object other)
            => ID == (other as Specialization)?.ID;

        #region Operators

        //public static bool operator ==(Specialization s1, Specialization s2)
        //{
        //    return s1.ID == s2.ID;
        //}

        //public static bool operator !=(Specialization s1, Specialization s2)
        //{
        //    return s1.ID != s2.ID;
        //}
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString() => ID + ": " +  specializationName;
    }
}