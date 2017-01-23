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
    /// Interaction logic for Edit_UserControl.xaml
    /// </summary>
    public partial class Edit_UserControl : UserControl
    {
        ContentControl tabButtonPane;

        public Edit_UserControl()
        {
            InitializeComponent();

            tabButtonPane = ((ContentControl)employerTabButtonPane.Content);
            Button[] employerButtons = new Button[3];
            int i = 0;
            foreach (var element in ((StackPanel)tabButtonPane.Content).Children)
            {
                if (element is Button)
                    employerButtons[i++] = (Button)element;
            }


            //binding of IsEnabled of add button for employer
            UserControl UserControl = (UserControl) FindName("employerUC");
            ComboBox cb = (ComboBox) UserControl.FindName("ComEmplyeID");

            Binding binding1 = new Binding();
            binding1.Source = cb;
            binding1.Path = new PropertyPath("Text");
            //binding1.Converter = ;
            employerButtons[0].SetBinding(Button.IsEnabledProperty, binding1);
        }

        private void addButtonEnableCheck()
        {
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Employer_UserControl_TextInput(object sender, TextCompositionEventArgs e)
        {
            MessageBox.Show(e.Text);
        }
    }
}
