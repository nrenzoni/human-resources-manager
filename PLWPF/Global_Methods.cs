using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PLWPF
{
    public static class Globals
    {
        //public static BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public static void exceptionHandler(Exception ex)
        {
            MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void CopyObject(object sourceObj, object targetObj)
        {
            if (sourceObj.GetType() != targetObj.GetType())
                throw new InvalidOperationException("cannot copy object of different type");

            // copy values (by use of property get/set) of foundEmployer to tempContract
            foreach (var property in sourceObj.GetType().GetProperties())
            {
                // check if property has set method (profit property does not have setter)
                if (property.GetSetMethod() != null)
                {
                    PropertyInfo propertyS = targetObj.GetType().GetProperty(property.Name);
                    var value = property.GetValue(sourceObj, null);
                    propertyS.SetValue(targetObj, value, null);
                }
            }
        }

        public static void ClearAllFields(Grid resetGrid)
        {
            foreach (var item in resetGrid.Children)
            {
                if (item.GetType() == typeof(ComboBox))
                    (item as ComboBox).SelectedIndex = -1;

                else if (item.GetType() == typeof(TextBox))
                    (item as TextBox).Text = "";

                else if (item.GetType() == typeof(DatePicker))
                    (item as DatePicker).Text = "";
            }
        }

        public static DateTime ResetDatePicker()
            => new DateTime(2000, 1, 1);          
              
    }
}
