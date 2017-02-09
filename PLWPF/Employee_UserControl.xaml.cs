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

        // exclude from setting the isEnabled property on these properties because they are binded (setting value of property in code behind after property is binded removes binding)
        string[] isEnabledExclusions = { "ComEmplyeeID", "txtBranCityLoc", "txtBranAddLoc" };

        State EmployeeUC_State = State.view;

        public Employee_UserControl()
        {
            InitializeComponent();

            DataContext = UIEmployee;
            
            ComEmplyeeCity.ItemsSource = from name in BE.CivicAddress.Cities
                                         orderby name
                                         select name;

            ComEmplyeeEduc.ItemsSource = Enum.GetValues(typeof(BE.Education));
            refreshComboBoxes();

            EmployeeGrid.setIsEnabled(false, isEnabledExclusions); // set isEnabled to false on all controls except for ComEmplyeeID
        }

        private void ComEmplyeeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Employee foundEmploye = Bl_Object.getEmployeeList().FirstOrDefault(x => x == (BE.Employee)ComEmplyeeID.SelectedItem);

            // if selected ID not found in DB, reset all fields in UIEmployee, thus resetting all controls in UI
            if (Equals(foundEmploye, null))
                Globals.CopyObject(new BE.Employee(), UIEmployee);

            else // selection exists in DB
            {
                Globals.CopyObject(foundEmploye, UIEmployee);
                ComEmplyeeBranchNum.SelectedItem = Bl_Object.getBankList().Find(b => b.BankName == UIEmployee.bank.BankName && b.Branch == UIEmployee.bank.Branch);
            }

        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            add_ButtonVisib();

            ComEmplyeeID.ItemsSource = null;

            EmployeeGrid.setIsEnabled(true, isEnabledExclusions);

            EmployeeUC_State = State.createNew;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (EmployeeUC_State)
                {
                    case State.createNew:
                        BE.Employee addEmploye = new BE.Employee();
                        Globals.CopyObject(UIEmployee, addEmploye);
                        Bl_Object.addEmployee(addEmploye);
                        Employee_DS_Change_Event?.Invoke();
                        break;

                    case State.modify:
                        Bl_Object.updateEmployee(UIEmployee);
                        Employee_DS_Change_Event?.Invoke();
                        break;
                }

                restoreRegButtonVisib();
                EmployeeUC_State = State.view;
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl_Object.deleteEmployee(UIEmployee);
                Employee_DS_Change_Event?.Invoke();
                restoreRegButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void add_ButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
        }

        private void restoreRegButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            // set isEnabled to false on all controls
            EmployeeGrid.setIsEnabled(false, isEnabledExclusions);
            ComEmplyeeID.IsEnabled = true;

            // refresh 
            refreshComboBoxes();
        }

        public void refreshComboBoxes()
        {
            ComEmplyeeID.ItemsSource = Bl_Object.getEmployeeList();
            comEmplyeBankName.ItemsSource = Bl_Object.getBanksGrouped();
            ComEmployeSpec.ItemsSource = Bl_Object.getSpecilizationList();
        }

        private void makeChangesButton_Click(object sender, RoutedEventArgs e)
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            
            updateButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;

            EmployeeGrid.setIsEnabled(true, isEnabledExclusions);
            ComEmplyeeID.IsEnabled = false;

            EmployeeUC_State = State.modify;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            restoreRegButtonVisib();
        }
    }
}
