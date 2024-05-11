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
    public partial class AddMember : Form
    {
        public AddMember()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=NexusLite-PC;Initial Catalog=gymdb;Integrated Security=True;Encrypt=False;");

        private void cbgender_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbtimeoftraining_Click(object sender, EventArgs e)
        {
            
        }

        private void AddMember_Load(object sender, EventArgs e)
        {
            getcoachesforthemember();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (txtMemName.Text.Trim() == String.Empty || txtMemAge.Text.Trim() == string.Empty
                || txtMemAmount.Text.Trim() == string.Empty || txtMemPhone.Text.Trim() == string.Empty
                || cbselectcoach.Text.Trim() == string.Empty || cbgender.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Missing Informations");
                return;
            }
            try
            {
                double amount = Convert.ToDouble(txtMemAmount.Text);
                int age = Convert.ToInt32(txtMemAge.Text);
                con.Open();

                // Insert payment
                string insertPaymentQuery = "INSERT INTO payments (amount, date) VALUES (@amount, @date); SELECT SCOPE_IDENTITY();";
                SqlCommand cmdInsertPayment = new SqlCommand(insertPaymentQuery, con);
                cmdInsertPayment.Parameters.AddWithValue("@amount", amount);
                cmdInsertPayment.Parameters.AddWithValue("@date", DateTime.Today);
                int paymentId = Convert.ToInt32(cmdInsertPayment.ExecuteScalar());

                // Insert user
                string insertUserQuery = "INSERT INTO users (name, age, phone, pid, coach_id, gender) VALUES (@mname, @mage, @mphone, @payid, @coachid, @mgender)";
                SqlCommand cmdInsertUser = new SqlCommand(insertUserQuery, con);
                cmdInsertUser.Parameters.AddWithValue("@mname", txtMemName.Text);
                cmdInsertUser.Parameters.AddWithValue("@mage", age);
                cmdInsertUser.Parameters.AddWithValue("@mphone", txtMemPhone.Text);
                cmdInsertUser.Parameters.AddWithValue("@payid", paymentId);
                cmdInsertUser.Parameters.AddWithValue("@coachid", Convert.ToInt32(cbselectcoach.SelectedValue));
                cmdInsertUser.Parameters.AddWithValue("@mgender", cbgender.Text);
                cmdInsertUser.ExecuteNonQuery();

                MessageBox.Show("User Added Successfully");
                resetmemb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void getcoachesforthemember()
        {
            con.Open();
            string query1 = "SELECT * FROM coach";
            SqlDataAdapter sda = new SqlDataAdapter(query1, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            cbselectcoach.DisplayMember = "name";
            cbselectcoach.ValueMember = "id";
            cbselectcoach.DataSource = ds.Tables[0];
            con.Close();
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            resetmemb();
        }

        private void resetmemb() 
        {
            txtMemAge.Clear();
            txtMemAmount.Clear();
            txtMemName.Clear();
            txtMemPhone.Clear();
            cbgender.SelectedIndex = -1;
            cbselectcoach.SelectedIndex = -1;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnback_Click(object sender, EventArgs e)
        {
            mainform mainform = new mainform();
            mainform.Show();
            this.Hide();
        }
    }
}
