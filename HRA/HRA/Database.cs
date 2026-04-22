using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HRA
{
    internal class Database
    {
        public static void DatabaseService(string ConnString, List<Employee> adatok) 
        {   

            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                conn.Open();


                // People table - Insert
                string createPeopleTable = "CREATE TABLE IF NOT EXISTS People (Id INT AUTO_INCREMENT PRIMARY KEY, FirstName VARCHAR(255), LastName VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createPeopleTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("People Tábla sikeresen létrehozva!");
                }

                string insertPeopleTable = "INSERT INTO People (FirstName, LastName) VALUES (@FirstName, @LastName)";
                using (MySqlCommand cmd = new MySqlCommand(insertPeopleTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++) 
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@FirstName", adatok[i].FirstName);
                        cmd.Parameters.AddWithValue("LastName", adatok[i].LastName);
                        cmd.ExecuteNonQuery();

                    }
                    Console.WriteLine("People tábla sikeresen feltöltve");
                }


                // Department table - Insert
                string createDepartmentTable = "CREATE TABLE IF NOT EXISTS Department (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createDepartmentTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Department Tábla sikeresen létrehozva!");
                }

                string insertDepartmentTable = "INSERT INTO Department (Name) VALUES (@Name)";
                using (MySqlCommand cmd = new MySqlCommand(insertDepartmentTable, conn))
                {
                    for(int i = 0;i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Name", adatok[i].Department);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Department tábla sikeresen feltöltve");
                }


                // Job Position Table - Insert
                string createJobPositionTable = "CREATE TABLE IF NOT EXISTS JobPosition (Id INT AUTO_INCREMENT PRIMARY KEY, JobName VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createJobPositionTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("JobPosition Tábla sikeresen létrehozva!");
                }

                string insertJobPositionTable = "INSERT INTO JobPosition (JobName) VALUES (@JobName)";
                using (MySqlCommand cmd = new MySqlCommand(insertJobPositionTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@JobName", adatok[i].JobTitle);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("JobPosition tábla sikeresen feltöltve");
                }


                // Employee Table - Insert
                string createEmployeeTable = "CREATE TABLE IF NOT EXISTS Employee (Id INT AUTO_INCREMENT PRIMARY KEY, GrossWage VARCHAR(255), NetWage VARCHAR(255), People_ID INT, JobPosition_ID INT, Department_ID INT, CONSTRAINT FK_Employee_PeopleID FOREIGN KEY (People_ID) REFERENCES People (Id), CONSTRAINT FK_Employee_DepartmentID FOREIGN KEY (Department_ID) REFERENCES Department (Id), CONSTRAINT FK_Employee_JobPositionID FOREIGN KEY (JobPosition_ID) REFERENCES JobPosition (Id))";
                using (MySqlCommand cmd = new MySqlCommand(createEmployeeTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Employee Tábla sikeresen létrehozva!");
                }

                string insertEmployeeTable = "INSERT INTO Employee (GrossWage, NetWage) VALUES (@GrossWage, @NetWage)";
                using (MySqlCommand cmd = new MySqlCommand(insertEmployeeTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@GrossWage", adatok[i].GrossWage);
                        cmd.Parameters.AddWithValue("@NetWage", adatok[i].NetWage);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Employee tábla sikeresen feltöltve");
                }


                // History Tabel - Insert
                string createHistoryTable = "CREATE TABLE IF NOT EXISTS History (People_ID INT, BeginDate DATE, EndDate DATE, CONSTRAINT FK_Hystory_PeopleID FOREIGN KEY (People_ID) REFERENCES People (Id))";
                using (MySqlCommand cmd = new MySqlCommand(createHistoryTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("History Tábla sikeresen létrehozva!");
                }

                string insertHistoryTable = "INSERT INTO History (BeginDate, EndDate) VALUES (@BeginDate, @EndDate)";
                using (MySqlCommand cmd = new MySqlCommand(insertHistoryTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@BeginDate", adatok[i].BeginDate);
                        cmd.Parameters.AddWithValue("@EndDate", adatok[i].EndDate);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("History tábla sikeresen feltöltve");
                }




                //People (Firtname, Lastname, id), Department(id, departmentname), JobPosition(name, id), employee(grosswage, netwage, jobpos id, department id, people id, id), hisytory(people id, begin, end) 
            }
        }
    }
}
