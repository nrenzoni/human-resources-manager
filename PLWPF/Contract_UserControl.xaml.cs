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
        BE.Contract tempContract = new BE.Contract();

        public event Action Contract_DS_Change_Event;

        private void refreshComboID()
            => comboContractID.ItemsSource = BL_Object.getContractList();

        public Contract_UserControl()
        {
            InitializeComponent();

            DataContext = tempContract;

            comboContractID.DataContext = BL_Object.getContractList();
            ComboEmployerID.DataContext = BL_Object.getEmployerList().Select(x => x.ID);
            ComboEmployeeID.DataContext = BL_Object.getEmployeeList().Select(x => x.ID);

            Contract_DS_Change_Event += refreshComboID;
        }

        public void selectContract(BE.Contract contract)
        {
            comboContractID.SelectedItem = contract;
        }

        private void addNewContract_Click(object sender, RoutedEventArgs e)
        {
            addNewContract_Button.Visibility = Visibility.Collapsed;
            SaveNewContract_Button.Visibility = Visibility.Visible;
            CancelNewContract_Button.Visibility = Visibility.Visible;
            TerminateContract_Button.Visibility = Visibility.Collapsed;
            FinalizeContract_Button.Visibility = Visibility.Collapsed;

            comboContractID.IsEnabled = false;
            ComboEmployerID.IsEnabled = true;
            ComboEmployeeID.IsEnabled = true;
            checkboxInterviewed.IsEnabled = true;

            txtGrossWage.IsEnabled =   true;
            txtNetWage.Text = "";
            txtMaxWorkHRS.IsEnabled =  true;
            DTtermDate.IsEnabled =     true;

            comboContractID.SelectedIndex = -1;
            comboContractID.Text = BL_Object.getNextContractID().ToString();
            tempContract.contractEstablishedDate = DateTime.Today;
            tempContract.contractFinalized = false; // finalized in later stage by user clicking button
        }

        private void saveNewContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var emplyerID = ComboEmployerID.SelectedValue;
                var employeeID = ComboEmployeeID.SelectedValue;
                if (emplyerID == null || employeeID == null)
                    throw new Exception("please fill out all fields");

                tempContract.EmployerID = (uint)emplyerID;
                tempContract.EmployeeID = (uint)employeeID;

                BE.Contract copyContract = new BE.Contract();
                Globals.CopyObject(tempContract, copyContract); // copy bc otherwise added by reference

                if (BL_Object.addContract(copyContract))
                {
                    Contract_DS_Change_Event?.Invoke(); // refreshes ContractList in ViewUC
                }
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
            CancelNewContract_Click(sender, e); // restores original buttons
        }

        private void CancelNewContract_Click(object sender, RoutedEventArgs e)
        {
            comboContractID.SelectedIndex = comboContractID.Items.Count -1; // sets selectedIndex to last contract

            addNewContract_Button.Visibility = Visibility.Visible;
            SaveNewContract_Button.Visibility = Visibility.Collapsed;
            CancelNewContract_Button.Visibility = Visibility.Collapsed;
            TerminateContract_Button.Visibility = Visibility.Visible;
            FinalizeContract_Button.Visibility = Visibility.Visible;

            comboContractID.IsEnabled = true;
            ComboEmployerID.IsEnabled = false;
            ComboEmployeeID.IsEnabled = false;
            checkboxFinalized.IsEnabled = false;
            checkboxInterviewed.IsEnabled = false;

            txtGrossWage.IsEnabled =  false;
            txtMaxWorkHRS.IsEnabled = false;
            DTtermDate.IsEnabled =    false;

            comboContractID.IsEnabled = true;
            ComboEmployerID.IsEnabled = false;
            ComboEmployeeID.IsEnabled = false;

            //comboContractID.SelectedIndex = 0;
        }

        // if comboContractID selection changed, update tempContract by performing copy, in effect triggering INotify on properties
        private void comboContractID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // check if no selection
            if (((ComboBox)sender).SelectedItem == null)
                return;

            BE.Contract newSelection = (BE.Contract)e.AddedItems[0];

            // copy values (by use of property get/set) of newSelection to tempContract
            Globals.CopyObject(newSelection, tempContract);
        }



        private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        {
            BE.Contract Contract_ref = BL_Object.getContractListByFilter(x => x.Equals(tempContract)).First();
            Contract_ref.contractFinalized = true;
            Globals.CopyObject(Contract_ref, tempContract);
            Contract_DS_Change_Event?.Invoke();
        }

        private void checkboxInterviewed_Checked(object sender, RoutedEventArgs e)
        {
            checkboxInterviewed.IsEnabled = false;
            BE.Contract Contract_ref = BL_Object.getContractListByFilter(x => x.Equals(tempContract)).First();
            Contract_ref.isInterviewed = true;
            Globals.CopyObject(Contract_ref, tempContract);
            Contract_DS_Change_Event?.Invoke();
        }
    }
}
