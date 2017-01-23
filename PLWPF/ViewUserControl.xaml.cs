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

        public ViewUserControl()
        {
            InitializeComponent();
            DataContext = BL_Object.getContractList();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ContractList.ItemsSource);
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
    }
}
