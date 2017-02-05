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
        public event Action<BE.Contract> onContractDoubleClick;

        public ViewUserControl()
        {
            InitializeComponent();
            refreshContractList();
        }

        public void refreshContractList()
        {
            group_selection_Changed();
        }

        // method goes over every object in listview. if returns true for current object, object is visible. otherwise not displayed.
        private bool Filter(object item)
        {
            if (filter_Selection == null) return true;

            // no filter selected,
            if (filter_Selection.SelectedIndex == -1 || filter_Selection.SelectedIndex == 0)
                return true;

            // filter only contracts that have not terminated
            else if(filter_Selection.SelectedIndex == 1)
            {
                if ((item as ContractGroupingContainer).contract.contractTerminatedDate > DateTime.Today)
                    return true;
            }

            // filter only contracts that have terminated already
            else if(filter_Selection.SelectedIndex == 2)
            {
                if ((item as ContractGroupingContainer).contract.contractTerminatedDate <= DateTime.Today)
                    return true;
            }

            return false;
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
            // refreshs filter
            CollectionViewSource.GetDefaultView(ContractList.ItemsSource).Refresh();
        }

        private void group_selection_Changed(object sender=null, SelectionChangedEventArgs e=null)
        {
            switch (group_Selection.SelectedIndex)
            {
                case 0: // no grouping
                    ContractList.ItemsSource = BL_Object.getContractsInContainer();
                    break;

                case 1: // Employer City
                    ContractList.ItemsSource = BL_Object.groupContractByEmployerCity(true); // returns sorted
                    break;

                case 2: // Employee City
                    ContractList.ItemsSource = BL_Object.groupContractByEmployeeCity(true); // sorted
                    break;

                case 3: // Employee Specialization
                    ContractList.ItemsSource = BL_Object.groupContractByEmployeeSpec(true); // sorted
                    break;

                default:
                    return;
            }

            // sorts contracts by "key" in ContractGroupingContainer
            CollectionView contractListView =
                (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
            contractListView.GroupDescriptions.Add(new PropertyGroupDescription("key"));
            contractListView.Filter = Filter;
        }
    }
}
