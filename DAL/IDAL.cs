using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    interface IDAL
    {
        bool addSpecilization(Specialization specilization);
        bool deleteSpecilization(Specialization specilization);
        bool updateSpecilization(Specialization specilization);

        bool addEmployee(Employee employee);
        bool deleteEmployee(Employee employee);
        bool updateEmployee(Employee employee);
    }
}
