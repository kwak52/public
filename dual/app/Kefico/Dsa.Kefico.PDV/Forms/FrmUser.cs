using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV.Forms
{
    public partial class FrmUser : Form
    {
        private DataTable User;
        public int UserId { get; set; }

        public FrmUser(DataTable dtUser)
        {
            User = dtUser;
            InitializeComponent();
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            foreach (DataRow dr in User.Rows)
                comboBoxEdit_User.Properties.Items.Add(dr["username"]);

            comboBoxEdit_User.SelectedIndex = 0;
        }

        private void simpleButton_Enter_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in User.Rows)
            {
                if (comboBoxEdit_User.SelectedItem == dr["username"])
                {
                    if (textEdit_Password.Text == dr["password"].ToString())
                    {
                        UserId = Convert.ToInt32(dr["id"]);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        if (labelControl_Result.Text.Length % 2 == 0)
                            labelControl_Result.Text = "Invalid password!";
                        else
                            labelControl_Result.Text = "Invalid password!!";
                    }
                }
            }
        }
    }
}
