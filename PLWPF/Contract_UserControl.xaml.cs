using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Contract_UserControl.xaml
    /// </summary>
    public partial class Contract_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;
        BE.Employee tempEmployee = new BE.Employee();

        public Contract_UserControl()
        {
            InitializeComponent();
            DataContext = tempEmployee;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try { BL_Object.addEmployee(tempEmployee); }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try { BL_Object.deleteEmployee(tempEmployee); }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try { BL_Object.updateEmployee(tempEmployee); }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }
    }
}
