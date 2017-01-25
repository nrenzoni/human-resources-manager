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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        public MainWindow()
        {
            InitializeComponent();
            editUC.DS_Edit_Event += EditUC_onChange;
            viewUC.onContractDoubleClick += ViewUC_onContractDoubleClick;
        }

        private void ViewUC_onContractDoubleClick(Contract obj)
        {
            mainTab.SelectedIndex = 1;
            editUC.open_EditUC_Tab(obj);
        }

        private void EditUC_onChange()
         => viewUC.refreshContractList();
    }
}
