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
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BL.IBL BL_Object = BL.FactoryBL.IBLInstance;

        BackgroundWorker DownloadBankXML = new BackgroundWorker();

        public event Action DownloadBankXMLCompleted;

        public MainWindow()
        {
            InitializeComponent();
            editUC.DS_Edit_Event += EditUC_onChange;
            viewUC.onContractDoubleClick += ViewUC_onContractDoubleClick;

            DownloadBankXMLCompleted += editUC.OnDownloadBankXMLCompleted;

            DownloadBankXML.DoWork += new DoWorkEventHandler(BL_Object.getXMLBankBackground_DoWork());
            DownloadBankXML.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getXMLBankRunner_Completed);
            DownloadBankXML.RunWorkerAsync(); // runs downloadBankXml asynchronously 
        }

        private void ViewUC_onContractDoubleClick(Contract obj)
        {
            mainTab.SelectedIndex = 1;
            editUC.open_EditUC_Tab(obj);
        }

        private void EditUC_onChange()
         => viewUC.refreshContractList();

        void getXMLBankRunner_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result?.ToString() == "downloadSuccess")
            {
                DownloadBankXMLCompleted?.Invoke();
                Globals.exceptionHandler(new Exception("download of Banks.xml succeeded"));
            }

            else if (e.Result?.ToString() == "loadSuccess")
            {
                DownloadBankXMLCompleted?.Invoke();
                Globals.exceptionHandler(new Exception("load of Banks.xml succeeded"));
            }

            else
            {
                Globals.exceptionHandler(new Exception("failed to initalize Banks.xml. Closing Program"));
                this.Close();
            }
        }
    }
}
