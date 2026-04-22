using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRA
{
    internal class CsvReader
    {
        public static List<Employee> CsvHelper(string path) 
        {
            List<Employee> employees = new List<Employee>();

            if (!File.Exists(path)) 
            {
                Console.WriteLine("A file nem található!");
                return employees;
            } 

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                string header = sr .ReadLine();
                while (!sr.EndOfStream) 
                {
                    line = sr .ReadLine().TrimEnd(';');
                    string[] splittedline = line.Split(',');
                    var employee = new Employee();
                    employee.FirstName = splittedline[0];
                    employee.LastName = splittedline[1];
                    employee.GrossWage = int.Parse(splittedline[2]);
                    employee.NetWage = int.Parse(splittedline[3]);
                    employee.JobTitle = splittedline[4];
                    employee.Department = splittedline[5];
                    employee.BeginDate = DateTime.Parse(splittedline[6]);
                    if (!string.IsNullOrEmpty(splittedline[7]))
                    {
                        employee.EndDate = DateTime.Parse(splittedline[7]);
                    }
                    employees.Add(employee);
                }
            }
            return employees;
        }
    }
}
