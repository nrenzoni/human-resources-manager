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
        GridViewColumnHeader listViewSortCol;
        bool descdingDir = false;
        IEnumerable<ContractGroupingContainer> unfilteredSource;
        public event Action<BE.Contract> onContractDoubleClick;

        public ViewUserControl()
        {
            InitializeComponent();
            unfilteredSource = BL_Object.getContractsInContainer();
        }

        public void refreshContractList()
        {
            group_selection_Changed(); // sets unfilteredSource
            filter_selection_Changed();


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
            Contract selectedContract = (ContractList.SelectedItem as ContractGroupingContainer)?.contract;
            if (selectedContract == null) return;
            onContractDoubleClick?.Invoke(selectedContract);
        }

        private void filter_selection_Changed(object sender=null, SelectionChangedEventArgs e=null)
        {
            if (filter_Selection == null) return; // when called at program initialization, filter_selection is null

            switch (filter_Selection.SelectedIndex)
            {
                case 0: // no filter
                    ContractList.ItemsSource = unfilteredSource;
                    break;

                case 1: // unterminated contracts
                    ContractList.ItemsSource = 
                        unfilteredSource.Where(contrGroup => contrGroup.contract.contractTerminatedDate > DateTime.Today);
                    break;

                case 2: // terminated contracts
                    ContractList.ItemsSource =
                        unfilteredSource.Where(contrGroup => contrGroup.contract.contractTerminatedDate <= DateTime.Today);
                    break;

                default:
                    break;
            }
        }

        private void group_selection_Changed(object sender=null, SelectionChangedEventArgs e=null)
        {
            CollectionView contractListView; //= (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);

            switch (group_Selection.SelectedIndex)
            {
                case 0: // no grouping
                    unfilteredSource = BL_Object.getContractsInContainer();
                    ContractList.ItemsSource = unfilteredSource;
                    contractListView = (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
                    contractListView?.GroupDescriptions.Clear();
                    return; // no GroupDescription added because of return

                case 1: // Employer City
                    ContractList.ItemsSource = BL_Object.groupContractByEmployerCity(true); // returns sorted
                    break;

                case 2: // Employee City
                    ContractList.ItemsSource = BL_Object.groupContractByEmployeeCity(true); // sorted
                    contractListView = (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
                    break;

                default:
                    return;
            }
            
            // sorts contracts by "key" in ContractGroupingContainer
            contractListView = (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
            contractListView.GroupDescriptions.Add(new PropertyGroupDescription("key"));
        }
    }
}
