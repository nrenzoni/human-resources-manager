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

        public event Action DS_Edit_Event;

        private void onUCupdate() => DS_Edit_Event?.Invoke();

        public void open_EditUC_Tab(object obj)
        {
            if (obj is BE.Contract)
            {
                tabC.SelectedIndex = 2; // contractUC
                contractUC.selectContract(obj as BE.Contract);
            }
        }

        public Edit_UserControl()
        {
            InitializeComponent();
            employerUC.Employer_DS_Change_Event += onUCupdate;
            contractUC.Contract_DS_Change_Event += onUCupdate;
        }
    }
}
