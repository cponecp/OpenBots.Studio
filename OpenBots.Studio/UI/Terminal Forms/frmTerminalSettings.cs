using OpenBots.Core.UI.Forms;
using System;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Forms
{
    public partial class frmTerminalSettings : UIForm
    {
        public string Host { get { return txtHost.Text; } }
        public int Port { get { return int.Parse(txtHostPort.Text); } }
        public string TerminalType { get { return txtTerminalType.Text; } }
        public bool UseSsl { get { return cbUseSSL.Checked; } }

        public frmTerminalSettings()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
