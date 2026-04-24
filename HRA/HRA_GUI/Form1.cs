using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                string selectJobTitle = "SELECT JobName FROM JobPosition JOIN Employee ON JobPosition_ID = JobPosition.Id WHERE FirstName = @FN";
                using (MySqlCommand cmd = new MySqlCommand())
                {

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
