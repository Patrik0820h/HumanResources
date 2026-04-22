using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRA
{
    internal class Employee
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public int GrossWage { get; set; }
        public int NetWage { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
