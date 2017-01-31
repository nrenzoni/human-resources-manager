using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class FactoryDSList
    {
        private static DS.List_Source ds_instance;

        private FactoryDSList() { }
        static FactoryDSList()
        {
            ds_instance = new List_Source();
        }

        public static List_Source DSInstance {  get { return ds_instance; } }
    }
}
