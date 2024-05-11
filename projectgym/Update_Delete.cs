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
    public partial class Update_Delete : Form
    {
        private DataSet ds;
        public Update_Delete()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;");
        private void getallmembers() 
        {
            try
            {
                con.Open();
                string query1 = "SELECT * FROM users";
                SqlDataAdapter sda = new SqlDataAdapter(query1, con);
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

        private void Update_Delete_Load(object sender, EventArgs e)
        {
            clear();
            listBox1.SelectedIndex = -1;
            getcoachesforthemember();
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

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear() 
        {
            txtname.Clear();
            txtamount.Clear();
            txtage.Clear();
            txtphone.Clear();
            search.Clear();
            getallmembers();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) // Check if an item is selected
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;
                txtname.Text = selectedRow["name"].ToString();
                txtage.Text = selectedRow["age"].ToString();
                txtphone.Text = selectedRow["phone"].ToString();
                string gender = selectedRow["gender"].ToString();
                cbgender.SelectedItem = gender;

                try
                {
                    con.Open();
                    string pid = selectedRow["pid"].ToString();
                    string query1 = "SELECT amount FROM payments WHERE id = @pid";
                    SqlCommand cmd = new SqlCommand(query1, con);
                    cmd.Parameters.AddWithValue("@pid", pid);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        txtamount.Text = result.ToString();
                    }
                    else
                    {
                        txtamount.Text = "No payment found"; 
                    }
                    int cbindex = Convert.ToInt32(selectedRow["coach_id"].ToString());
                    //This code iterates through each item in cbcoach.Items. For each item, it casts it to a DataRowView object to access its properties. It then compares the id property (assumed as the value member) with cbindex. If a match is found, that item is set as the selected item in the cbcoach ComboBox using cbcoach.SelectedItem = item;.
                    foreach (var item in cbcoach.Items)
                    {
                        DataRowView coachRow = item as DataRowView;
                        if (coachRow != null && Convert.ToInt32(coachRow["id"]) == cbindex)
                        {
                            cbcoach.SelectedItem = item;
                            break;
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                txtname.Clear();
                txtamount.Clear();
                txtage.Clear();
                txtphone.Clear();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //hon etaret 8ayer el tari2a la2an el connection ma kenet 3m ta3mel close
            if (listBox1.SelectedIndex != -1) // Check if an item is selected
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;

                int userId = Convert.ToInt32(selectedRow["id"]);
                int pid = Convert.ToInt32(selectedRow["pid"]);
                string query3 = "UPDATE payments SET amount = @amount WHERE id = @payid";
                string updateQuery = "UPDATE users SET name = @name, age = @age, phone = @phone, gender = @gender, coach_id = @coach_id WHERE id = @userId";
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;"))
                    {
                        using (SqlCommand Cmd3 = new SqlCommand(query3, con))
                        {
                            Cmd3.Parameters.AddWithValue("@amount", txtamount.Text);
                            Cmd3.Parameters.AddWithValue("@payid", pid);
                            con.Open();
                            int rowsAffected = Cmd3.ExecuteNonQuery();
                            con.Close();
                        }

                        using (SqlCommand Cmd4 = new SqlCommand(updateQuery, con))
                        {
                            Cmd4.Parameters.AddWithValue("@name", txtname.Text);
                            Cmd4.Parameters.AddWithValue("@age", txtage.Text);
                            Cmd4.Parameters.AddWithValue("@phone", txtphone.Text);
                            Cmd4.Parameters.AddWithValue("@gender", cbgender.SelectedItem.ToString());
                            Cmd4.Parameters.AddWithValue("@coach_id", ((DataRowView)cbcoach.SelectedItem)["id"]);
                            Cmd4.Parameters.AddWithValue("@userId", userId);

                            con.Open();

                            // Execute the update query
                            int rowsAffected = Cmd4.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User information updated successfully.");
                                // Refresh the list after update
                                getallmembers();
                            }
                            else
                            {
                                MessageBox.Show("Update failed.");
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

        private void getcoachesforthemember()
        {
            con.Open();
            string query1 = "SELECT * FROM coach";
            SqlDataAdapter sda = new SqlDataAdapter(query1, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            cbcoach.DisplayMember = "name";
            cbcoach.ValueMember = "id";
            cbcoach.DataSource = ds.Tables[0];
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) // Check if an item is selected
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;
                int userId = Convert.ToInt32(selectedRow["id"]);
                string deleteQuery = "DELETE FROM users WHERE id = @userId";
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;"))
                    {
                        using (SqlCommand Cmd5 = new SqlCommand(deleteQuery, con))
                        {
                            Cmd5.Parameters.AddWithValue("@userId", userId);

                            con.Open();
                            int rowsAffected = Cmd5.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User deleted successfully.");
                                getallmembers();
                            }
                            else
                            {
                                MessageBox.Show("Delete failed.");
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

        private void txtamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainform mainform = new mainform();
            mainform.Show();
            this.Hide();
        }
    }
}
