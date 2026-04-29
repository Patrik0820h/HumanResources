using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRA_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connString = "server=localhost;port=3307;database=employee;uid=root";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string selectEmployees = "SELECT FirstName, LastName, GrossWage, NetWage FROM Employee";
                using (MySqlCommand cmd = new MySqlCommand(selectEmployees, conn))
                {
                    var reader = cmd.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);

                    dataGridView1.DataSource = table;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }

            var row = dataGridView1.Rows[e.RowIndex];

            string firstName = row.Cells["FirstName"].Value.ToString();
            string lastName = row.Cells["LastName"].Value.ToString();
            int grossWage = Convert.ToInt32(row.Cells["GrossWage"].Value);

            string connString = "server=localhost;port=3307;database=employee;uid=root";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string selectJobTitle = @"SELECT JobName FROM JobPosition JOIN Employee ON JobPosition.Id = JobPosition_ID WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW";
                using (MySqlCommand cmd = new MySqlCommand(selectJobTitle, conn))
                {
                    cmd.Parameters.AddWithValue("@FN", firstName);
                    cmd.Parameters.AddWithValue("@LN", lastName);
                    cmd.Parameters.AddWithValue("@GW", grossWage);

                    MySqlDataReader result = cmd.ExecuteReader();

                    if (result != null)
                    {
                        result.Read();
                        label1.Text = "Munkakör: " + result.GetString(0).Trim();
                    }
                    else
                    {
                        MessageBox.Show("Nincs találat");
                    }
                    result.Close();
                }

                string selectDepartmentName = @"SELECT Name FROM Department JOIN Employee ON Department.Id = Department_ID WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW";
                using (MySqlCommand cmd = new MySqlCommand(selectDepartmentName, conn))
                {
                    cmd.Parameters.AddWithValue("@FN", firstName);
                    cmd.Parameters.AddWithValue("@LN", lastName);
                    cmd.Parameters.AddWithValue("@GW", grossWage);

                    MySqlDataReader result = cmd.ExecuteReader();

                    if (result != null)
                    {
                        result.Read();
                        label2.Text = "Department: " + result.GetString(0).Trim();
                    }
                    else
                    {
                        MessageBox.Show("Nincs találat");
                    }
                    result.Close();
                }
            }
        }
    }
}
