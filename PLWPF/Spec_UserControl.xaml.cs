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
            refreshCombox();
        }


        private void refreshCombox()
            => ComSpecID.ItemsSource = BL_Object.getSpecilizationList();

        // only called when new ID selected from combobox list, if value entered is not in combobox list, not called
        private void ComSpecID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // reflection for copying properties from selected Spec in ID field to properties of UISpec
            if (ComSpecID.SelectedItem == null) // check if null because uint cast potentially on null
                return;

            BE.Specialization foundSpec = 
                BL_Object.getSpecilizationList().FirstOrDefault(x => x == (BE.Specialization)ComSpecID.SelectedItem);
            
            // if selected ID does not exist in DS, return
            if (ReferenceEquals(null, foundSpec))
            {
                //foundSpec = new BE.Specialization();
                return;
            }

            // copy values (by use of property get/set) of foundSpec to UISpec so binding to UISpec not reset
            Globals.CopyObject(foundSpec, UISpec);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BE.Specialization addSpec = new BE.Specialization();
                Globals.CopyObject(UISpec, addSpec);
                BL_Object.addSpecialization(addSpec);
                Spec_DS_Change_Event?.Invoke();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
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
