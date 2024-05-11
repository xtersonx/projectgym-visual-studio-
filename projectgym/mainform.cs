using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projectgym
{
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
        }

        private void btnaddmember_MouseEnter(object sender, EventArgs e)
        {
            btnaddmember.BackColor = Color.Black;
            btnaddmember.ForeColor = Color.White;
        }

        private void btnaddmember_MouseLeave(object sender, EventArgs e)
        {
            btnaddmember.BackColor = Color.MediumTurquoise;
            btnaddmember.ForeColor = Color.Black;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnaddmember_Click(object sender, EventArgs e)
        {
            AddMember addMember = new AddMember();
            addMember.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addcoach addcoach = new addcoach();
            addcoach.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update_Delete update_Delete = new Update_Delete();
            update_Delete.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            payments payments = new payments();
            payments.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackColor = Color.Black;
            button4.ForeColor = Color.White;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.MediumTurquoise;
            button4.ForeColor = Color.Black;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.ForeColor = Color.White;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.MediumTurquoise;
            button1.ForeColor = Color.Black;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.Black;
            button3.ForeColor = Color.White;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.MediumTurquoise;
            button3.ForeColor = Color.Black;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.Black;
            button2.ForeColor = Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.MediumTurquoise;
            button2.ForeColor = Color.Black;
        }
    }
}
