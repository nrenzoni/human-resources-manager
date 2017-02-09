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
    enum State { view, createNew, modify }

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

        public static DateTime ResetDatePicker()
            => new DateTime(1970, 1, 1);          
              
    }

    public static class ExtensionMethods
    {
        public static bool hasProperty(this object ObjectToCheck, string methodName)
        {
            var type = ObjectToCheck.GetType();
            return type.GetProperty(methodName) != null;
        }

        public static void setIsEnabled(this object parent, bool val, params string[] exclude)
        {
            // set isEnabled to val on all chilren and children's children etc recursively
            if (!parent.hasProperty("Children"))
                throw new Exception("cannot perform setIsEnabled on parent that does not have Children");

            foreach (var control in (UIElementCollection)parent.GetType().GetProperty("Children").GetValue(parent, null))
            {
                // if sub-child elements exist, recursive call
                if (control.hasProperty("Children"))
                    setIsEnabled(control, val, exclude);

                // check exclusion rule by 'Name' property of controls
                bool skip = false;
                foreach (var excludeRule in exclude)
                {
                    if (control.hasProperty("Name"))
                    {
                        if (control.GetType().GetProperty("Name").GetValue(control).ToString() == excludeRule)
                        {
                            skip = true;
                            break;
                        }
                    }
                }

                if (skip)
                    continue;

                // finally, set isEnabled property to val of current control if it has IsEnabled property
                if (control.hasProperty("IsEnabled"))
                    control.GetType().GetProperty("IsEnabled").SetValue(control, val);
            }
        }
    }
}
