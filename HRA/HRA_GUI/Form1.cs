using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.Http;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO;
using Org.BouncyCastle.Asn1.Kisa;
using System.Diagnostics;

namespace HRA_GUI
{
    public partial class Form1 : Form
    {
        int lastSelectedRow;
        public Dictionary<int, Image> cachedImages = new Dictionary<int, Image>();
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
                    dataGridView1.ReadOnly = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == lastSelectedRow) { return; }

            lastSelectedRow = e.RowIndex;
            var selectedRow = dataGridView1.Rows[e.RowIndex];

            string firstName = selectedRow.Cells["FirstName"].Value.ToString();
            string lastName = selectedRow.Cells["LastName"].Value.ToString();
            int grossWage = Convert.ToInt32(selectedRow.Cells["GrossWage"].Value);

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
                        label1.Text = result.GetString(0).Trim();
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
                        label2.Text = result.GetString(0).Trim();
                    }
                    else
                    {
                        MessageBox.Show("Nincs találat");
                    }
                    result.Close();
                    GetCatPicture("https://cataas.com/cat", e.RowIndex);
                    
                }
            }
        }
        private readonly HttpClient Client = new HttpClient();
        public void GetCatPicture(string url, int rowIndex) 
        {
            if (cachedImages[rowIndex] == null)
            {
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] responseBody = response.Content.ReadAsByteArrayAsync().Result;
                    Stream picture = new MemoryStream(responseBody);
                    pictureBox1.Image = Image.FromStream(picture);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    cachedImages.Add(rowIndex, pictureBox1.Image);
                }
                else
                {
                    Console.WriteLine($"Hiba: {response.StatusCode}");
                }
            }
            else 
            {
                pictureBox1.Image = cachedImages[rowIndex];
            }
        }
    }
}
