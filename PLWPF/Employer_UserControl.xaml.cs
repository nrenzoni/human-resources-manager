using PLWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class Employer_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        BE.Employer UIEmployer = new BE.Employer();

        public event Action Employer_DS_Change_Event;

        State EmployerUC_State = State.view;

        // exclude from setting the isEnabled property on these properties because they are binded (setting value of property in code behind after property is binded removes binding)
        string[] isEnabledExclusions = { "ComEmployerID", "txtEmplyeFirstName", "txtEmplyeLastName" };

        public Employer_UserControl()
        {
            InitializeComponent();
            Employer_DS_Change_Event += refreshCombox;
            DataContext = UIEmployer;

            ComEmplyeCity.ItemsSource = from name in BE.CivicAddress.Cities
                                        orderby name
                                        select name;
            refreshCombox();

            EmployerGrid.setIsEnabled(false, isEnabledExclusions);

            restoreButtonVisib();
        }

        public void refreshCombox()
        {
            ComEmployerID.ItemsSource = BL_Object.getEmployerList();
            ComEmplyeSpec.ItemsSource = BL_Object.getSpecilizationList();
        }

        // only called when new ID selected from combobox list, if value entered is not in combobox list, not called
        private void ComEmployerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Employer foundEmployer = BL_Object.getEmployerList().FirstOrDefault(x => x == (BE.Employer)ComEmployerID.SelectedItem);

            if (Equals(foundEmployer, null)) // check if null because uint cast potentially on null
            {
                // resets UIEmployer fields, in effect reseting all controls in UI bc of binding
                Globals.CopyObject(new BE.Employer(), UIEmployer);
                return;
            }

            // copy values (by use of property get/set) of foundEmployer to UIEmployer so binding to UIEmployer not reset
            else { Globals.CopyObject(foundEmployer, UIEmployer); }
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            add_ButtonVisib();

            ComEmployerID.ItemsSource = null;

            EmployerGrid.setIsEnabled(true, "ComEmployerID", "txtEmplyeFirstName", "txtEmplyeLastName");

            EmployerUC_State = State.createNew;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (EmployerUC_State)
                {

                    case State.createNew:
                        BE.Employer addEmployer = new BE.Employer();
                        Globals.CopyObject(UIEmployer, addEmployer);
                        BL_Object.addEmployer(addEmployer);
                        break;

                    case State.modify:
                        BL_Object.updateEmployer(UIEmployer);
                        Employer_DS_Change_Event?.Invoke();
                        break;

                    default:
                        throw new Exception("EmployerUC State not set");

                }

                Employer_DS_Change_Event?.Invoke();
                restoreButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            restoreButtonVisib();
            ComEmployerID.ItemsSource = BL_Object.getEmployerList();

            EmployerGrid.setIsEnabled(false, isEnabledExclusions);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.deleteEmployer(UIEmployer);
                Employer_DS_Change_Event?.Invoke();
                restoreButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void makeChangesButton_Click(object sender, RoutedEventArgs e)
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;

            updateButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;

            EmployerGrid.setIsEnabled(true, isEnabledExclusions);
            ComEmployerID.IsEnabled = false;

            EmployerUC_State = State.modify;
        }


        void add_ButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
        }

        void restoreButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            // set isEnabled to false on all children in grid
            EmployerGrid.setIsEnabled(false, isEnabledExclusions);
            ComEmployerID.IsEnabled = true;
        }

        private void cBoxPrivate_Checked(object sender, RoutedEventArgs e)
        {
            // resets first and last name if private person
            UIEmployer.firstName = "";
            UIEmployer.lastName  = "";
        }
    }
}
