﻿using System;
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
    /// Interaction logic for UserControltest1.xaml
    /// </summary>
    public partial class ViewUserControl : UserControl
    {
        public ViewUserControl()
        {
            var currentApp = System.Windows.Application.Current as App;

            InitializeComponent();

            try
            {
                employeeList.ItemsSource = currentApp.BL_Object.getEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}