using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BE;
using BL;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    public class IDToIsEnabledConverter : IValueConverter
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BE.converterParams converterParamAsEnum;

            int IDtoCheck;

            // if value is not int, disable add button
            if (value == null || int.TryParse(value.ToString(), out IDtoCheck) != true)
                return "False";

            // check if invalid parameter
            if (Enum.TryParse(parameter.ToString(), out converterParamAsEnum) == false)
                throw new Exception("bad parameter passed to IDTo_IsEnabled_Converter");

            switch (converterParamAsEnum)
            {
                // ID is for employer
                case BE.converterParams.Employer:
                    if (BL_Object.getEmployerList().ToList().Exists(e => e.ID == IDtoCheck))
                        return "False"; // can't add if item exists
                    return "True";

                case BE.converterParams.Employee:
                    if (BL_Object.getEmployeeList().ToList().Exists(e => e.ID == IDtoCheck))
                        return "False"; // can't add if item exists
                    return "True";

                case BE.converterParams.Spec:
                    if(BL_Object.getSpecilizationList().Exists(s => s.ID == IDtoCheck))
                        return "False"; // can't add if item exists
                    return "True";

                default:
                    break;
            }
            return "False";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("IDToIsEnabledConverter does not implement ConvertBack");
        }
    }

    // flip IDToIsEnabledConverter
    public class InverseIDToIsEnabledConverter : IValueConverter
    {
        // if int in box, true, else false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int IDtoCheck;

            // if value is not int, disable add button
            if (value == null || int.TryParse(value.ToString(), out IDtoCheck) != true)
                return "False";

            var temp = new IDToIsEnabledConverter().Convert(value, targetType, parameter, culture);
            return (string)temp == "True" ? "False" : "True";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("InverseIDToIsEnabledConverter does not implement ConvertBack");
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool))
            {
                return (bool)value ? "False" : "True";
            }
            else throw new Exception("cannot perform InverseBoolConverter on non-bool type");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new InverseBoolConverter().Convert(value, targetType, parameter, culture);
        }
    }

    public class ContractTerminatedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(DateTime))
            {
                // if contract hasn't terminated, return visible for 'Terminate' Button
                if (((DateTime)value).Date > DateTime.Today)
                    return "Visible";
                else
                    return "Collapsed";
            }

            throw new Exception("ContractTerminatedConverter only accepts value of type DateTime");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class specIDToSpecObjectConverter : IValueConverter
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return BL_Object.getSpecilizationList().Find(s => s.ID == (uint)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as BE.Specialization).ID;
        }
    }

    public class EmployerIDToEmployerObjectConverter : IValueConverter
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BL_Object.getEmployerList().Find(e => e.ID == (uint)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return (value as BE.Employer).ID;
        }
    }

    public class EmployeeIDToEmployeeObjectConverter : IValueConverter
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BL_Object.getEmployeeList().Find(e => e.ID == (uint)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return (value as BE.Employee).ID;
        }
    }

    public class NoValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            Type type = value.GetType();

            if (type == typeof(string))
                if (string.IsNullOrEmpty(value.ToString()))
                    return "";

            if (type.IsValueType) // check if struct type
                if (Equals(value, Activator.CreateInstance(type))) // check if equal to default, ex for int, 0
                    return "";

            // no match return regular value
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
