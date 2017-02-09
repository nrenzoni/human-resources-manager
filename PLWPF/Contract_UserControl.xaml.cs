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
    public partial class Contract_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;
        BE.Contract UIContract = new BE.Contract();

        public event Action Contract_DS_Change_Event;

        // exclude from setting the isEnabled property on these properties because they are binded (setting value of property in code behind after property is binded removes binding)
        string[] isEnabledExclusions = { "comboContractID", "txtNetWage" };

        public void refreshComboxesIDs()
        {
            comboContractID.ItemsSource = BL_Object.getContractList();
            ComboEmployerID.ItemsSource = BL_Object.getEmployerList(); // uses ToString() for display in combobox
            ComboEmployeeID.ItemsSource = BL_Object.getEmployeeList(); // uses ToString() for display in combobox
        }

        // triggers INotify since copy
        void updateUIContract(BE.Contract newContract)
            => Globals.CopyObject(newContract, UIContract);

        public Contract_UserControl()
        {
            InitializeComponent();

            DataContext = UIContract;
            
            refreshComboxesIDs();

            Contract_DS_Change_Event += refreshComboxesIDs;
        }

        public void selectContract(BE.Contract contract)
        {
            comboContractID.SelectedItem = contract;
        }

        private void addNewContract_Click(object sender, RoutedEventArgs e)
        {
            add_ButtonVisibState();

            comboContractID.IsEnabled = false;
            contractUCGrid.setIsEnabled(true, isEnabledExclusions);

            updateUIContract(new BE.Contract()); // resets all controls in UI
            comboContractID.Text = BL_Object.getNextContractID().ToString();
        }

        private void saveNewContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var emplyer = (BE.Employer)ComboEmployerID.SelectedValue;
                var employee = (BE.Employee)ComboEmployeeID.SelectedValue;
                if (emplyer == null || employee == null)
                    throw new Exception("please fill out all fields");

                UIContract.EmployerID = emplyer.ID;
                UIContract.EmployeeID = employee.ID;

                UIContract.contractID = BL_Object.getNextContractID();

                BE.Contract copyContract = new BE.Contract();
                Globals.CopyObject(UIContract, copyContract); // copy bc otherwise added by reference

                BL_Object.addContract(copyContract); // exception thrown if failed add
                Contract_DS_Change_Event?.Invoke(); // refreshes ContractList in ViewUC
                restoreButtonVisibState(); // restore buttons if add succeeded
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void CancelNewContract_Click(object sender, RoutedEventArgs e)
        {
            comboContractID.SelectedIndex = comboContractID.Items.Count -1; // sets selectedIndex to last contract

            restoreButtonVisibState(); // sets isEnabled to false on all UI controls
        }

        // if comboContractID selection changed, update tempContract by performing copy, in effect triggering INotify on properties
        private void comboContractID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // check if no selection
            if ((sender as ComboBox)?.SelectedItem == null)
                updateUIContract(new BE.Contract()); // reset UIcontract, thus reseting UI controls

            else
                updateUIContract(e.AddedItems[0] as BE.Contract); // updates UIContract with new selection
        }

        private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        {
            BE.Contract Contract_ref = BL_Object.getContractListByFilter(x => Equals(x,UIContract)).FirstOrDefault();
            if (Contract_ref == null)
                return;

            Contract_ref.contractFinalized = true;
            updateUIContract(Contract_ref);
            Contract_DS_Change_Event?.Invoke();
        }

        private void checkboxInterviewed_Checked(object sender, RoutedEventArgs e)
        {
            //checkboxInterviewed.IsEnabled = false;
            //BE.Contract Contract_ref = BL_Object.getContractListByFilter(x => Equals(x,UIContract)).FirstOrDefault();
            //Contract_ref.isInterviewed = true;
            //Globals.CopyObject(Contract_ref, UIContract);
            //Contract_DS_Change_Event?.Invoke();
        }

        private void TerminContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BL_Object.terminateContract(UIContract) == false) // contract not found in DB
                    return; 

                Contract_DS_Change_Event?.Invoke();

                // refresh UI elements
                int selectedi = comboContractID.SelectedIndex;
                comboContractID.SelectedIndex = -1;
                comboContractID.SelectedIndex = selectedi;

                throw new Exception("Contract terminated successfuly");
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        void add_ButtonVisibState()
        {
            addNewContract_Button.Visibility = Visibility.Collapsed;
            SaveNewContract_Button.Visibility = Visibility.Visible;
            CancelNewContract_Button.Visibility = Visibility.Visible;
            TerminateContract_Button.Visibility = Visibility.Collapsed;
            FinalizeContract_Button.Visibility = Visibility.Collapsed;
        }

        void restoreButtonVisibState()
        {
            addNewContract_Button.Visibility = Visibility.Visible;
            SaveNewContract_Button.Visibility = Visibility.Collapsed;
            CancelNewContract_Button.Visibility = Visibility.Collapsed;
            TerminateContract_Button.Visibility = Visibility.Visible;
            FinalizeContract_Button.Visibility = Visibility.Visible;

            comboContractID.IsEnabled = true;
            contractUCGrid.setIsEnabled(false, isEnabledExclusions);
        }
    }
}
