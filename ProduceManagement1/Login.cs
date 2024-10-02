using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProduceManagement1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void chkbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtbLoginPassword.PasswordChar = chkbShowPassword.Checked ? '\0' : '*';
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtbLoginUsername.Text == "" || txtbLoginPassword.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtbLoginUsername.Text == "Admin" && txtbLoginPassword.Text == "Password")
            {
                Produce MainForm = new Produce();
                MainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect Username/Password",
                                "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
