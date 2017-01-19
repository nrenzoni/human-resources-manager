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
    public class MarginSetter
    {
        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(MarginSetter), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        public static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Make sure this is put on a panel
            var panel = sender as Panel;

            if (panel == null) return;


            panel.Loaded += new RoutedEventHandler(panel_Loaded);

        }

        static void panel_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = sender as Panel;

            // Go over the children and set margin for them:
            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;

                if (fe == null) continue;

                fe.Margin = MarginSetter.GetMargin(panel);
            }
        }
    }



    /// <summary>
    /// Interaction logic for Employer_UserControl.xaml
    /// </summary>
    public partial class Employer_UserControl : UserControl
    {
        public BL.IBL BL_Object = BL.FactoryBL.IBLInstance;
        BE.Employer tempEmployer = new BE.Employer{companyName="comp" };

        public Employer_UserControl()
        {
            InitializeComponent();
            DataContext = tempEmployer;

            ComEmplyeCity.ItemsSource = BE.CivicAddress.Cities;
            ComEmplyeSpec.ItemsSource = from spec in BL_Object.getSpecilizationList()
                                  select spec.specilizationName;
            ComEmplyeID.ItemsSource = from emp in BL_Object.getEmployerList()
                                select emp.ID;
        }

        // only called when new ID selected from combobox list
        private void ComEmplyeID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // reflection for copying properties from selected Employer in ID field to properties of tempEmployer
            if (ComEmplyeID.SelectedItem == null) // check if null because uint cast potentially on null
                return;
            
            BE.Employer selectedEmployer = BL_Object.getEmployerList().FirstOrDefault(x => x.ID == (uint)ComEmplyeID.SelectedItem);
            

            if (selectedEmployer.Equals( null))
            {
                selectedEmployer = new BE.Employer();
                return;
            }

            foreach (var property in selectedEmployer.GetType().GetProperties())
            {
                PropertyInfo propertyS = tempEmployer.GetType().GetProperty(property.Name);
                var value = property.GetValue(selectedEmployer, null);
                propertyS.SetValue(tempEmployer, property.GetValue(selectedEmployer, null), null);
            }
        }
    }
}
