using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IBL BL_Object = FactoryBL.IBLInstance;

        public Contract tempContract = new Contract();
        public Employee tempEmployee = new Employee();
        public Employer tempEmployer = new Employer();
    }
}
