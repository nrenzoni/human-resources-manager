using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class FactoryDSList
    {
        private static DS.DataSource ds_instance;

        private FactoryDSList() { }
        static FactoryDSList() { ds_instance = new DataSource(); }

        public static DataSource DSInstance {  get { return ds_instance; } }
    }
}
