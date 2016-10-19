using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.RestartMe
{
    public partial class frmDebug : Form
    {
        private svcMain _service;
        public frmDebug()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
#if DEBUG
            _service = new svcMain();
            _service.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
#endif
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
#if DEBUG
            _service.Stop();
            _service.Dispose();
            _service = null;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
#endif
        }
    }
}
