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
    /// Interaction logic for UserControltest1.xaml
    /// </summary>
    public partial class ViewUserControl : UserControl
    {
        public IBL BL_Object = FactoryBL.IBLInstance;

        public void refreshContractList()
        {
            switch (filter_Selection.SelectedIndex)
            {
                case 0: // no filter
                    ContractList.ItemsSource = BL_Object.getContractList();
                    break;
                case 1: // unterminated contracts
                    ContractList.ItemsSource = BL_Object.getContractListByFilter(c => c.contractTerminatedDate > DateTime.Today);
                    break;
                case 2: // terminated contracts
                    ContractList.ItemsSource = BL_Object.getContractListByFilter(c => c.contractTerminatedDate < DateTime.Today);
                    break;
                default:
                    break;
            }
        }

        GridViewColumnHeader listViewSortCol;
        bool descdingDir = false;

        public event Action<BE.Contract> onContractDoubleClick;

        public ViewUserControl()
        {
            InitializeComponent();

            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
        }

        /// <summary>
        /// event triggered when user clicks on one of listview header, sorts by clicked on header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContractListHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);

            string sortBy = column.Tag.ToString();

            if (listViewSortCol != null) // previous sorting applied
            {
                ContractList.Items.SortDescriptions.Clear();
            }

            listViewSortCol = column;

            ListSortDirection newDir = ListSortDirection.Descending;

            // if true, previous sorting applied to clicked on column in descending direction
            if (listViewSortCol == column && descdingDir) 
            {
                descdingDir = false;
                newDir = ListSortDirection.Ascending;
            }
            else
            {
                descdingDir = true;
            }

            ContractList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void ContractList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedContract = (BE.Contract)ContractList.SelectedItem;
            onContractDoubleClick?.Invoke(selectedContract);
        }

        private void filter_selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            switch (filter_Selection.SelectedIndex)
            {
                case 0: // no filter
                    ContractList.ItemsSource = BL_Object.getContractList();
                    break;
                case 1: // unterminated contracts
                    ContractList.ItemsSource = BL_Object.getContractListByFilter(c => c.contractTerminatedDate > DateTime.Today);
                    break;
                case 2: // terminated contracts
                    ContractList.ItemsSource = BL_Object.getContractListByFilter(c => c.contractTerminatedDate < DateTime.Today);
                    break;
                default:
                    break;
            }
        }
    }
}
