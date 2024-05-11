using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace projectgym
{
    public partial class addcoach : Form
    {
        public addcoach()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;");


        private void btnadd_Click(object sender, EventArgs e)
        {
            if (txtcoachname.Text.Trim() == String.Empty || txtcoachnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Missing Information/s");
                return;
            }
            try
            {
                con.Open();
                string query = "INSERT INTO coach (name,phone) VALUES (@coachname, @coachnumber)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@coachname", txtcoachname.Text);
                cmd.Parameters.AddWithValue("@coachnumber", txtcoachnumber.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Coach Added Succesfuly");
                con.Close();
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void getcoaches() 
        {
            con.Open();
            string query1 = "select * from coach";
            SqlDataAdapter sda = new SqlDataAdapter(query1, con);
            SqlCommandBuilder builder = new SqlCommandBuilder();
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }

        private void addcoach_Load(object sender, EventArgs e)
        {
            getcoaches();
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            txtcoachname.Clear();
            txtcoachnumber.Clear();
        }

        private void clear() 
        {
            txtcoachname.Clear();
            txtcoachnumber.Clear();
            getcoaches();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            mainform mainform = new mainform();
            mainform.Show();
            this.Hide();
        }
    }
}
