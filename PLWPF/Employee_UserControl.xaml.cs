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
using System.Reflection;

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
            UIEmployee.birthday = Globals.ResetDatePicker();
        }

        private void ComEmplyeeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Employee foundEmploye = Bl_Object.getEmployeeList().FirstOrDefault(x => x == (BE.Employee)ComEmplyeeID.SelectedItem);

            // if selected ID not found in DB, reset all fields in UIEmployee, thus resetting all controls in UI
            if (Equals(foundEmploye, null))
                Globals.CopyObject(new BE.Employee(), UIEmployee);

            else Globals.CopyObject(foundEmploye, UIEmployee);
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            add_ButtonVisib();

            ComEmplyeeID.ItemsSource = null;
        }

        private void addSecondButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                BE.Employee addEmploye = new BE.Employee();
                Globals.CopyObject(UIEmployee, addEmploye);
                Bl_Object.addEmployee(addEmploye);
                Employee_DS_Change_Event?.Invoke();
                restoreButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            restoreButtonVisib();
            ComEmplyeeID.ItemsSource = Bl_Object.getEmployeeList();
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

        private void add_ButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            addSecondButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
        }

        private void restoreButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Visible;
            addSecondButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;
        }
    }
}
