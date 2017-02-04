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

        public Employer_UserControl()
        {
            InitializeComponent();
            Employer_DS_Change_Event += refreshCombox;
            DataContext = UIEmployer;

            ComEmplyeCity.ItemsSource = BE.CivicAddress.Cities;
            refreshCombox();
            //Globals.ClearAllFields(EmployerGrid);

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

                //Globals.ClearAllFields(EmployerGrid); // Clear the fields in the current grid.
                return;
            }

            // copy values (by use of property get/set) of foundEmployer to UIEmployer so binding to UIEmployer not reset
            else { Globals.CopyObject(foundEmployer, UIEmployer); }
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            add_ButtonVisib();

            ComEmployerID.ItemsSource = null;

            //EmployerGrid.setIsEnabled(true, "ComEmployerID", "txtEmplyeFirstName", "txtEmplyeLastName");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BE.Employer addEmployer = new BE.Employer();
                Globals.CopyObject(UIEmployer, addEmployer);
                BL_Object.addEmployer(addEmployer);
                Employer_DS_Change_Event?.Invoke();
                restoreButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            restoreButtonVisib();
            ComEmployerID.ItemsSource = BL_Object.getEmployerList();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.deleteEmployer(UIEmployer);
                Employer_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.updateEmployer(UIEmployer);
                Employer_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }


        void add_ButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Collapsed;
            addSecondButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
        }

        void restoreButtonVisib()
        {
            addFirstButton.Visibility = Visibility.Visible;
            addSecondButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            // set isEnabled to false on all children in grid
            //EmployerGrid.setIsEnabled(false, "ComEmployerID", "txtEmplyeFirstName", "txtEmplyeLastName");
        }

        private void cBoxPrivate_Checked(object sender, RoutedEventArgs e)
        {
            // resets first and last name if private person
            UIEmployer.firstName = "";
            UIEmployer.lastName  = "";
        }
    }
}
