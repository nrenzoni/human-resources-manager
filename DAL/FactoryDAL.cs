using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDAL
    {
        private static IDAL dal_instance;

        private FactoryDAL() { }
        static FactoryDAL() { dal_instance = new DAL_Imp_List(); }
        
        public static IDAL Instance { get { return dal_instance; } }
    }
}
