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
        BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public Edit_UserControl()
        {
            InitializeComponent();

            #region button binding (old)
            /*string[] UCs = { "employerUC", "employeeUC", "contractUC", "specUC" }; // UserControl Names in Edit_UserControl
        string[] srcElement = { "ComEmployerID", "ComEmplyeeID", "", "" }; // source element names to bind to
             
            Button[,] allTabButtons = new Button[4, 3];


            ContentControl[] tabButtonPanes = { (ContentControl)employerTabButtonPane.Content,
                                                (ContentControl)employeeTabButtonPane.Content,
                                                (ContentControl)contractTabButtonPane.Content,
                                                (ContentControl)specTabButtonPane.Content      };

            //binding of template buttons to matching UserControl
            for (int i = 0; i < 4; i++) // loop thru each buttonPane
            {
                for (int j = 0; j < 3; j++) // loop thru each button in pane
                {
                    // j'th child of current tabButtonPane
                    var templateElement = ((StackPanel)tabButtonPanes[i].Content).Children[j];
                    if (templateElement is Button)
                        allTabButtons[i, j] = (Button)templateElement;
                    else throw new Exception("change template binding logic"); // for safety

                    UserControl currentUC = (UserControl)FindName(UCs[i]);
                    if (currentUC == null)
                        throw new Exception("UserControl in Binding logic set to null object");

                    Binding b = new Binding
                    {
                        Source = (ComboBox)currentUC.FindName(srcElement[i]),
                        Path = new PropertyPath("Text"), // what type for spec?
                        ConverterParameter = Enum.GetNames(typeof(BE.converterParams))[i]
                    };
                    if (j == 0) b.Converter = new IDToIsEnabledConverter(); // add button
                    else b.Converter = new InverseIDToIsEnabledConverter(); // delete and update button

                    allTabButtons[i, j].SetBinding(Button.IsEnabledProperty, b);
                }
            }*/
            #endregion
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int selectedI = tabC.SelectedIndex;
            // check no tab selected
            if (selectedI == -1) return;

            switch (selectedI)
            {
                case 1: // employer tab

                default:
                    break;
            }
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
