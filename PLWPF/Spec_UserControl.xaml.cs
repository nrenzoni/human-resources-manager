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
    /// Interaction logic for Spec_UserControl.xaml
    /// </summary>
    public partial class Spec_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        BE.Specialization UISpec = new BE.Specialization();

        public event Action Spec_DS_Change_Event;

        public Spec_UserControl()
        {
            InitializeComponent();
            Spec_DS_Change_Event += refreshCombox;
            DataContext = UISpec;
            refreshCombox(); // refreshes comspecID itemssource
        }


        private void refreshCombox()
            => ComSpecID.ItemsSource = BL_Object.getSpecilizationList();

        private void ComSpecID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // check if null because uint cast potentially on null
            if (ComSpecID.SelectedItem == null || ComSpecID.SelectedIndex == -1)
            {
                Globals.ClearAllFields(SpecializationGrid);
                return;

            }          
            else
                Globals.CopyObject((BE.Specialization)ComSpecID.SelectedItem, UISpec);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            finalizeButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;

            txtSpecName.IsEnabled = true;
            txtMinWagePerHour.IsEnabled = true;
            txtMaxWagePerHour.IsEnabled = true;

            ComSpecID.IsEnabled = false;
            
            ComSpecID.Text = BL_Object.getNextContractID().ToString();
        }

        private void finalizeButton_Click(object sender, RoutedEventArgs e)
        {
            UISpec.ID = uint.Parse(ComSpecID.Text);

            try
            {
                BE.Specialization addSpec = new BE.Specialization();
                Globals.CopyObject(UISpec, addSpec);
                BL_Object.addSpecialization(addSpec);

                lockFields();

                Spec_DS_Change_Event?.Invoke(); // refreshes combobox as well
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            lockFields();
        }

        void lockFields()
        {
            addButton.Visibility = Visibility.Visible;
            finalizeButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            txtSpecName.IsEnabled = false;
            txtMinWagePerHour.IsEnabled = false;
            txtMaxWagePerHour.IsEnabled = false;

            ComSpecID.IsEnabled = true;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.deleteSpecilization(UISpec);
                Spec_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Object.updateSpecilization(UISpec);
                Spec_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }
    }
}
