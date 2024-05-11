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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Xml.Linq;

namespace projectgym
{
    public partial class payments : Form
    {
        private DataSet ds;
        public payments()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;");
        
        private void getallmembers()
        {
            try
            {
                DateTime date = DateTime.Today.AddDays(-30);
                con.Open();
                string query = "SELECT p.*, u.name, u.id FROM payments p JOIN users u ON p.id = u.pid WHERE p.date <= @date";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@date", date);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds, "Users");
                con.Close();
                listBox1.DataSource = ds.Tables["Users"];
                listBox1.DisplayMember = "name";
                listBox1.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) // Check if an item is selected
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;

                try
                {
                    con.Open();
                    string pid = selectedRow["id"].ToString();
                    string query1 = "SELECT amount, date FROM payments WHERE id = @pid";
                    SqlCommand cmd = new SqlCommand(query1, con);
                    cmd.Parameters.AddWithValue("@pid", pid);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Retrieve amount
                        txtamount.Text = reader["amount"].ToString();
                        // Retrieve date
                        DateTime date = Convert.ToDateTime(reader["date"]);
                        txtdate.Text = date.ToString("yyyy-MM-dd"); // or any other format you prefer
                    }
                    else
                    {
                        txtamount.Text = "No payment found";
                        txtdate.Text = ""; // Clear date textbox if no payment found
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (search.Text == String.Empty)
            {
                MessageBox.Show("Please enter a name.");
            }
            else
            {
                try
                {
                    // Filter the dataset based on the search text
                    DataRow[] filteredRows = ds.Tables["Users"].Select("name LIKE '%" + search.Text + "%'");

                    // Create a new DataTable to hold the filtered data
                    DataTable filteredTable = ds.Tables["Users"].Clone();
                    foreach (DataRow row in filteredRows)
                    {
                        filteredTable.ImportRow(row);
                    }

                    // Clear and rebind the DataSource property of the ListBox
                    listBox1.DataSource = null;
                    listBox1.Items.Clear();
                    listBox1.DataSource = filteredTable;
                    listBox1.DisplayMember = "name"; // DisplayMember should be set again
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void payments_Load(object sender, EventArgs e)
        {
            getallmembers();
        }

        private void clear() 
        {
            txtamount.Clear();
            txtdate.Clear();
            search.Clear();
            getallmembers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;                
                int pid = Convert.ToInt32(selectedRow["id"]);
                string query3 = "UPDATE payments SET date = @date WHERE id = @payid";
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;"))
                    {
                        using (SqlCommand Cmd3 = new SqlCommand(query3, con))
                        {
                            Cmd3.Parameters.AddWithValue("@date", DateTime.Today);
                            Cmd3.Parameters.AddWithValue("@payid", pid);
                            con.Open();
                            int rowsAffected = Cmd3.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("payment updated successfully.");
                                // Refresh the list after update
                                getallmembers();
                            }
                            else
                            {
                                MessageBox.Show("failed payment.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a user from the list.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainform mainform = new mainform();
            mainform.Show();
            this.Hide();
        }
    }
}
