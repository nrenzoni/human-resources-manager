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
            ComEmployeSpec.ItemsSource = (from word in (Enum.GetNames(typeof(BE.SpecializationName)))
                                         select word.Replace("_", " ")).ToList();
           

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
                List<object> checkValue = new List<object> ();

                var employeID = ComEmplyeeID.SelectedValue;
                checkValue.Add(employeID);

                var employeLName = txtLastName.Text;
                checkValue.Add(employeLName);

                var employeFName = txtFirstName.Text;
                checkValue.Add(employeFName);

                var employePhoneNum = int.Parse(txtPhoneNumber.Text);
                checkValue.Add(employePhoneNum);

                var employeAddress = txtEmplyeeAdd.Text;
                checkValue.Add(employeAddress);

                var employeCity = ComEmplyeeCity.SelectedValue;
                checkValue.Add(employeCity);

                var employeExpYears = txtExperiance.Text;
                checkValue.Add(employeExpYears);

                var employeEdu = ComEmplyeeEduc.SelectedItem;
                checkValue.Add(employeEdu);

                var employeSpec = ComEmployeSpec.SelectedItem;
                checkValue.Add(employeSpec);

                //NEED TO IMPLEMENT THE CHECKS IF THE BANK DETAILS ARE EMPTY, 
                //    WHEN FINISH TO CONNECT THE XML!~!~!

                foreach (var item in checkValue)
                {
                    if (item==null)
                        throw new Exception("please fill out all fields");
                }
                
                UIEmployee.ID = (uint)employeID;
                UIEmployee.lastName = employeLName;
                UIEmployee.firstName = employeFName;
                UIEmployee.phoneNumber = (uint)employePhoneNum;
                UIEmployee.address.Address = employeAddress;
                UIEmployee.address.City =(string)employeCity;
                UIEmployee.yearsOfExperience = uint.Parse(employeExpYears);
               // UIEmployee.education = Enum. employeEdu;
                //UIEmployee.specializationID = employeSpec;





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
