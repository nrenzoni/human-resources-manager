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
        Button addButton;

        public Edit_UserControl()
        {
            InitializeComponent();
        }

        private void addButtonEnableCheck()
        {
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // need to initialize vars if this works
            tabButtonPane = ((ContentControl)employerTabButtonPane.Content);
            Button[] employerButtons = new Button[3];
            int i = 0;
            foreach (Button button in ((StackPanel)tabButtonPane.Content).Children)
            { employerButtons[i++] = button; }
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
