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
    /// Interaction logic for Contract_UserControl.xaml
    /// </summary>
    public partial class Contract_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;
        BE.Employee tempEmployee = new BE.Employee();

        public Contract_UserControl()
        {
            InitializeComponent();
            DataContext = tempEmployee;
        }

        private void addNewContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                finalizeNewContract_Button.Visibility = Visibility.Visible;
                CancelNewContract_Button.Visibility = Visibility.Visible;
                TerminateContract_Button.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void CancelNewContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                finalizeNewContract_Button.Visibility = Visibility.Collapsed;
                CancelNewContract_Button.Visibility = Visibility.Collapsed;
                TerminateContract_Button.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }
    }
}
