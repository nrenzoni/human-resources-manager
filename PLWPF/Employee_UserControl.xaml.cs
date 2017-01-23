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
        BE.Employee UIEmployee = new BE.Employee();

        public Employee_UserControl()
        {
            InitializeComponent();

            DataContext = UIEmployee;

            ComEmplyeeCity.ItemsSource = BE.CivicAddress.Cities;
            ComEmplyeeID.ItemsSource = from emplye in Bl_Object.getEmployeeList()
                                       select emplye.ID;
            ComEmplyeeEduc.ItemsSource = Enum.GetValues(typeof(BE.Education));
        }

        private void ComEmplyeeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
