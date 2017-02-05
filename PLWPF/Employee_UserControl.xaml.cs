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
            
            ComEmplyeeCity.ItemsSource = from name in BE.CivicAddress.Cities
                                         orderby name
                                         select name;

            ComEmplyeeID.ItemsSource = Bl_Object.getEmployeeList();
            ComEmplyeeEduc.ItemsSource = Enum.GetValues(typeof(BE.Education));
            ComEmployeSpec.ItemsSource = Bl_Object.getSpecilizationList();
            UIEmployee.birthday=Globals.ResetDatePicker();
            comEmplyeBankName.ItemsSource = (from b in Bl_Object.getBankList()
                                             select b.BankName).Distinct();

            ComEmplyeeBranchNum.IsEnabled = false;

      


            //ComEmployeSpec.ItemsSource = from word in (Enum.GetNames(typeof(BE.SpecializationName)))
            //                             select word.Replace("_", " ");
            
        }

        private void ComEmplyeeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.Employee foundEmploye = Bl_Object.getEmployeeList().FirstOrDefault(x => x == (BE.Employee)ComEmplyeeID.SelectedItem);

            if (BE.Employee.Equals(foundEmploye, null))
            {
                Globals.ClearAllFields(EmployeeGrid); // Clear the fields in the current grid.
                return;
            }

            else Globals.CopyObject(foundEmploye, UIEmployee);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //foreach (var property in UIEmployee.GetType().GetProperties())
                //{
                //    if (property.Name != "recommendationNotes")
                //    {
                //        if (property.GetGetMethod() != null) //Check if Get Method are exist in the specific properity
                //        {
                //            if (property.GetValue(UIEmployee) == null)
                //                throw new Exception("please fill out all fields");
                //        }
                //    }
                //}

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

        private void comEmplyeBankName_SelectoinChanged(object sender, SelectionChangedEventArgs e)
        {
          
            ComEmplyeeBranchNum.IsEnabled = true;
            ComEmplyeeBranchNum.ItemsSource = (from b in Bl_Object.getBankList()
                                              where b.BankName == comEmplyeBankName.SelectedItem.ToString()
                                              orderby b.Branch
                                              select b.Branch).Distinct();
           
        }

        private void ComEmplyeeBranchNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.CivicAddress tmpLocation = new BE.CivicAddress();

            tmpLocation = (from b in Bl_Object.getBankList()
                           where b.Branch== uint.Parse(ComEmplyeeBranchNum.SelectedItem.ToString())
                           select b.Address).FirstOrDefault();

            txtBranAddLoc.Text = tmpLocation.Address;
            txtBranCityLoc.Text = tmpLocation.City;

        }

        
       
    }
}
