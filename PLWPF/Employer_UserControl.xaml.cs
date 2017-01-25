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

        BE.Employer tempEmployer = new BE.Employer();

        public event Action Employer_DS_Change_Event;

        public Employer_UserControl()
        {
            InitializeComponent();
            Employer_DS_Change_Event += refreshCombox;
            DataContext = tempEmployer;

            ComEmplyeCity.ItemsSource = BE.CivicAddress.Cities;
            refreshCombox();
        }

        private void refreshCombox()
        {
            ComEmployerID.ItemsSource = BL_Object.getEmployerList();
            ComEmplyeSpec.ItemsSource = from spec in BL_Object.getSpecilizationList()
                                        select spec.specilizationName;
        }

        // only called when new ID selected from combobox list, if value entered is not in combobox list, not called
        private void ComEmployerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // reflection for copying properties from selected Employer in ID field to properties of tempEmployer
            if (ComEmployerID.SelectedItem == null) // check if null because uint cast potentially on null
                return;
            
            BE.Employer foundEmployer = BL_Object.getEmployerList().FirstOrDefault(x => x == (BE.Employer)ComEmployerID.SelectedItem);
            

            if (foundEmployer.Equals( null))
            {
                foundEmployer = new BE.Employer();
                return;
            }

            // copy values (by use of property get/set) of foundEmployer to tempEmployer so binding to tempEmployer not reset
            Globals.CopyObject(foundEmployer, tempEmployer);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BE.Employer addEmployer = new BE.Employer();
                Globals.CopyObject(tempEmployer, addEmployer);
                BL_Object.addEmployer(addEmployer); // added as reference, therefore copy needed
                Employer_DS_Change_Event?.Invoke(); // invoked if no exception thrown from addEmployer method
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.deleteEmployer(tempEmployer);
                Employer_DS_Change_Event?.Invoke();
            }
            catch(Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                BL_Object.updateEmployer(tempEmployer);
                Employer_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }
    }
}
