using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace AnilistExport
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Text = "Anilist Export v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Disable button
            btnExport.Enabled = false;

            //MessageBox.Show("Wait for task to finish...");

            string UserID = GlobalFunc.AnilistGetUserId(txtUser.Text);
            if (!String.IsNullOrWhiteSpace(UserID))
            {
                GlobalFunc.WriteFile("userid.json", UserID);
                //MessageBox.Show("Result is not null!");
            }

            // Enable button
            btnExport.Enabled = true;
        }
    }
}
