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
    /// Interaction logic for Employee_UserControl.xaml
    /// </summary>
    public partial class Employee_UserControl : UserControl
    {
        public BL.IBL Bl_Object = BL.FactoryBL.IBLInstance;
        public event Action Employee_DS_Change_Event;

        BE.Employee UIEmployee = new BE.Employee();        

        public Employee_UserControl()
        {
            InitializeComponent();

            DataContext = UIEmployee;

            ComEmplyeeCity.ItemsSource = BE.CivicAddress.Cities;
            ComEmplyeeID.ItemsSource = Bl_Object.getEmployeeList();
            ComEmplyeeEduc.ItemsSource = Enum.GetValues(typeof(BE.Education));
            ComEmployeSpec.ItemsSource = Bl_Object.getSpecilizationList();

            //ComEmployeSpec.ItemsSource = from word in (Enum.GetNames(typeof(BE.SpecializationName)))
            //                             select word.Replace("_", " ");


        }

        private void ComEmplyeeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Employee foundEmploye = Bl_Object.getEmployeeList().FirstOrDefault(x => x == (BE.Employee)ComEmplyeeID.SelectedItem);

            if (foundEmploye == null)
            {
                foundEmploye = new BE.Employee();
                return;
            }

            Globals.CopyObject(foundEmploye, UIEmployee);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BE.Employee addEmploye = new BE.Employee();
                Globals.CopyObject(UIEmployee, addEmploye);
                Bl_Object.addEmployee(addEmploye);
                Employee_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl_Object.deleteEmployee(UIEmployee);
                Employee_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                Bl_Object.updateEmployee(UIEmployee);
                Employee_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }        
        }
    }
}
