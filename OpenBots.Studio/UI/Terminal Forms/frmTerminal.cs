using Open3270;
using OpenBots.Core.UI.Forms;
using System;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Forms
{
    public partial class frmTerminal : UIForm
    {
        private frmTerminalSettings settings = new frmTerminalSettings();
        private string _host;
        private int _port;
        private string _terminalType;
        private bool _useSsl;
        public TNEmulator TN3270 { get; set; }

        public frmTerminal(string host, int port, string terminalType, bool useSsl)
        {
            _host = host;
            _port = port;
            _terminalType = terminalType;
            _useSsl = useSsl;

            InitializeComponent();
        }

        public frmTerminal()
        {
            InitializeComponent();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            settings.ShowDialog();

            if (settings.DialogResult == DialogResult.OK)
            {
                _host = settings.Host;
                _port = settings.Port;
                _terminalType = settings.TerminalType;
                _useSsl = settings.UseSsl;
            }
        }

        public void btnConnect_Click(object sender, EventArgs e)
        {
            OpenEmulator.Connect(_host, _port, _terminalType, _useSsl);
            TN3270 = OpenEmulator.TN3270;
        }

        private void OpenEmulator_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            OpenEmulator.Disconnect();
            TN3270 = null;
        }

        private void frmTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenEmulator.Disconnect();
        }

        private void OpenEmulator_SelectionChanged(object sender, EventArgs e)
        {
            lblCoordinates.Text = $"Coordinates: {OpenEmulator.Coordinates}";
        }
    }
}
