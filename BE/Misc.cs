using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // converter uses param to check matching field
    public enum converterParams { Employer, Employee, Contract, Spec };

    public delegate void DoWorkDelegate(object sender, DoWorkEventArgs e);
}
