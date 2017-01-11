using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactoryBL
    {
        private static IBL bl_instance;

        private FactoryBL() { }
        static FactoryBL() { bl_instance = new BL_Imp(); } // called once at program startup

        public static IBL IBLInstance { get { return bl_instance; } }
    }
}
