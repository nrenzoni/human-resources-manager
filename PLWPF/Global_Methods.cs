using System;
using System.Collections.Generic;
using System.Linq;
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
            MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Exception Caught!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
