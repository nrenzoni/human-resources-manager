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

        State specUC_State = State.view;

        // exclude from setting the isEnabled property on these properties because they are binded (setting value of property in code behind after property is binded removes binding)
        string[] isEnabledExclusions = { "ComSpecID" };

        public event Action Spec_DS_Change_Event;

        public Spec_UserControl()
        {
            InitializeComponent();
            Spec_DS_Change_Event += refreshCombox;
            DataContext = UISpec;
            refreshCombox(); // initializes comspecID itemssource
        }


        private void refreshCombox()
            => ComSpecID.ItemsSource = BL_Object.getSpecilizationList();

        private void ComSpecID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if selection is not one of the specs in lcombobox
            if (ComSpecID.SelectedItem == null)
                Globals.CopyObject(new BE.Specialization(), UISpec);
            else
                Globals.CopyObject((BE.Specialization)ComSpecID.SelectedItem, UISpec);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            save_buttonVisib();

            SpecializationGrid.setIsEnabled(true, isEnabledExclusions);
            ComSpecID.IsEnabled = false;

            specUC_State = State.createNew;
            
            ComSpecID.Text = BL_Object.getNextSpecID().ToString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            UISpec.ID = uint.Parse(ComSpecID.Text);

            try
            {
                switch (specUC_State)
                {

                    case State.createNew:
                        BE.Specialization addSpec = new BE.Specialization();
                        Globals.CopyObject(UISpec, addSpec);
                        BL_Object.addSpecialization(addSpec);
                        Spec_DS_Change_Event?.Invoke();
                        break;

                    case State.modify:
                        BL_Object.updateSpecilization(UISpec);
                        Spec_DS_Change_Event?.Invoke();
                        break;

                }

                Spec_DS_Change_Event?.Invoke(); // refreshes combobox as well
                restoreButtonVisib();
            }
            catch (Exception ex) { Globals.exceptionHandler(ex); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            restoreButtonVisib();
            specUC_State = State.view;
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
            specUC_State = State.modify;

            update_buttonVisib();

            ComSpecID.IsEnabled = false;
            SpecializationGrid.setIsEnabled(true, isEnabledExclusions);
        }

        void update_buttonVisib()
        {
            addButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Collapsed;
        }

        void save_buttonVisib()
        {
            addButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
        }

        void restoreButtonVisib()
        {
            addButton.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

            SpecializationGrid.setIsEnabled(false, isEnabledExclusions);
            ComSpecID.IsEnabled = true;

        }
    }
}
