using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                    var DepartmentID = adatok.Select(dep => dep.Department).Distinct();
                    foreach (var item in DepartmentID)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Name", item);
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
                    var jobposdis = adatok.Select(jobpos => jobpos.JobTitle).Distinct().OrderBy(jobpos => jobpos);
                    foreach (var item in jobposdis)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@JobName", item);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("JobPosition tábla sikeresen feltöltve");
                }


                // Employee Table - Insert
                string createEmployeeTable = "CREATE TABLE IF NOT EXISTS Employee (Id INT AUTO_INCREMENT PRIMARY KEY, FirstName VARCHAR(255), LastName VARCHAR(255), GrossWage VARCHAR(255), NetWage VARCHAR(255), JobPosition_ID INT, Department_ID INT, CONSTRAINT FK_Employee_DepartmentID FOREIGN KEY (Department_ID) REFERENCES Department (Id), CONSTRAINT FK_Employee_JobPositionID FOREIGN KEY (JobPosition_ID) REFERENCES JobPosition (Id))";
                using (MySqlCommand cmd = new MySqlCommand(createEmployeeTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Employee Tábla sikeresen létrehozva!");
                }

                string insertEmployeeTable = "INSERT INTO Employee (FirstName, LastName, GrossWage, NetWage, JobPosition_ID, Department_ID) VALUES (@FirstName, @LastName, @GrossWage, @NetWage, @JobPosition_ID, @Department_ID)";
                string SelectJobPosID = "SELECT Id FROM JobPosition WHERE JobName = @JobTitle";
                string SelectDepID = "SELECT Id FROM Department WHERE Name = @Dp";
                using (MySqlCommand cmd = new MySqlCommand(insertEmployeeTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        int jobtitleid;
                        int depid;
                        using (MySqlCommand cmd2 = new MySqlCommand(SelectJobPosID, conn))
                        {
                            cmd2.Parameters.AddWithValue("@JobTitle", adatok[i].JobTitle);
                            jobtitleid = (int)cmd2.ExecuteScalar();
                        }
                        using (MySqlCommand cmd3 = new MySqlCommand(SelectDepID, conn))
                        {
                            cmd3.Parameters.AddWithValue("@Dp", adatok[i].Department);
                            depid = (int)cmd3.ExecuteScalar();
                        }
                        cmd.Parameters.AddWithValue("@FirstName", adatok[i].FirstName);
                        cmd.Parameters.AddWithValue("@LastName", adatok[i].LastName);
                        cmd.Parameters.AddWithValue("@GrossWage", adatok[i].GrossWage);
                        cmd.Parameters.AddWithValue("@NetWage", adatok[i].NetWage);
                        cmd.Parameters.AddWithValue("@JobPosition_ID", jobtitleid);
                        cmd.Parameters.AddWithValue("@Department_ID", depid);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Employee tábla sikeresen feltöltve");
                }


                // History Table - Insert
                string createHistoryTable = "CREATE TABLE IF NOT EXISTS History (BeginDate DATE, EndDate DATE, EmployeeID INT, CONSTRAINT FK_History_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employee (Id))";
                using (MySqlCommand cmd = new MySqlCommand(createHistoryTable, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("History Tábla sikeresen létrehozva!");
                }

                string insertHistoryTable = "INSERT INTO History (BeginDate, EndDate, EmployeeID) VALUES (@BeginDate, @EndDate, @EmployeeID)";
                string SelectEmployeeID = "SELECT Employee.Id FROM Employee JOIN JobPosition ON JobPosition_ID = JobPosition.Id WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW AND JobPosition.JobName = @JN";
                using (MySqlCommand cmd = new MySqlCommand(insertHistoryTable, conn))
                {
                    for (int i = 0; i <= adatok.Count - 1; i++)
                    {
                        cmd.Parameters.Clear();
                        int EmpID;
                        using (MySqlCommand cmd2 = new MySqlCommand(SelectEmployeeID, conn))
                        {
                            cmd2.Parameters.AddWithValue("@FN", adatok[i].FirstName);
                            cmd2.Parameters.AddWithValue("@LN", adatok[i].LastName);
                            cmd2.Parameters.AddWithValue("@GW", adatok[i].GrossWage);
                            cmd2.Parameters.AddWithValue("@JN", adatok[i].JobTitle);
                            EmpID = (int)cmd2.ExecuteScalar();
                        }
                        cmd.Parameters.AddWithValue("@BeginDate", adatok[i].BeginDate);
                        cmd.Parameters.AddWithValue("@EndDate", adatok[i].EndDate);
                        cmd.Parameters.AddWithValue("@EmployeeID", EmpID);
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("History tábla sikeresen feltöltve");
                }
            }
        }
    }
}
