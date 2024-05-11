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
    public partial class login : Form
    {
        string adminName;
        string adminPassword;
        public login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;");

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string query = "SELECT username, password FROM admin";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            var ds = new DataSet();
            sda.Fill(ds);
            con.Close();

            bool found = false; // Flag to track if a matching username and password are found

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string adminName = row["username"].ToString();
                string adminPassword = row["password"].ToString();

                if (txtuser.Text == adminName && txtpass.Text == adminPassword)
                {
                    found = true;
                    break; // Exit the loop if a match is found
                }
            }

            if (txtuser.Text.Trim() == String.Empty || txtpass.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Missing Information/s");
            }
            else if (found)
            {
                mainform main = new mainform();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }
    }
}
