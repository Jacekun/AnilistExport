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
    }
}
