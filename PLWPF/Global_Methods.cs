using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLWPF
{
    public static class Globals
    {
        //public static BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public static void exceptionHandler(Exception ex)
        {
            MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static  void CopyObject(object inObj, object outObj)
        {
            if (inObj.GetType() != outObj.GetType())
                throw new InvalidOperationException("cannot copy object of different type");

            // copy values (by use of property get/set) of foundEmployer to tempContract
            foreach (var property in inObj.GetType().GetProperties())
            {
                // check if property has set method (profit property does not have setter)
                if (property.GetSetMethod() != null)
                {
                    PropertyInfo propertyS = outObj.GetType().GetProperty(property.Name);
                    var value = property.GetValue(inObj, null);
                    propertyS.SetValue(outObj, value, null);
                }
            }
        }
    }
}
