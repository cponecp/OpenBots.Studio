using Open3270;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Drawing;
using System.Linq;
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
            OpenEmulator.InitializeComponent();
            Size = new Size(OpenEmulator.Size.Width + 20, OpenEmulator.Height + msTerminal.Height + 50);

            connectToolStripMenuItem_Click(null, null);
        }

        public frmTerminal()
        {
            InitializeComponent();
            OpenEmulator.InitializeComponent();
            Size = new Size(OpenEmulator.Size.Width + 20, OpenEmulator.Height + msTerminal.Height + 50);
        }

        public void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEmulator.Connect(_host, _port, _terminalType, _useSsl);
            TN3270 = OpenEmulator.TN3270;
            Text = $"Terminal (Connected) - Host: {_host} - Port: {_port}";
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void OpenEmulator_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEmulator.Disconnect();
            TN3270 = null;
            Text = "Terminal (Disconnected)";
        }

        private void frmTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenEmulator.Disconnect();
            OpenEmulator.Dispose();
        }

        private void OpenEmulator_SelectionChanged(object sender, EventArgs e)
        {
            coordinatesToolStripMenuItem.Text = $"Coord: {OpenEmulator.Coordinates}";
            fieldIndexToolStripMenuItem.Text = $"Field Index: {OpenEmulator.FieldIndex}";
        }

        public void fieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fields = StringMethods.ConvertListToString(TN3270.CurrentScreenXML.Fields.ToList());
            MessageBox.Show(fields, "Terminal Fields");
        }

        public delegate void CloseFormDelegate();
        public void CloseForm()
        {
            if (InvokeRequired)
            {
                var d = new CloseFormDelegate(CloseForm);
                Invoke(d, new object[] { });
            }
            else
            {
                Close();
            }
        }
    }
}
