using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string DbConnString = "server=localhost;port=3307;database=employee;uid=root";
            Database.DatabaseService(DbConnString);
        }
    }
}
