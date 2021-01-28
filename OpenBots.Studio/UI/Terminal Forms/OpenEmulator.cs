using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Open3270;
using OpenBots.Utilities;
using OpenBots.Commands.Terminal.Library;

namespace OpenBots.Commands.Terminal.Forms
{
    public class OpenEmulator : RichTextBox
    {
        public TNEmulator TN3270 = new TNEmulator();
        private bool IsRedrawing = false;
        public Point Coordinates { get; set; } = new Point(0, 0);
        public void Connect(string Server, int Port, string Type, bool UseSsl)
        {
            TN3270.Config.UseSSL = UseSsl;
            TN3270.Config.TermType = Type;
            TN3270.Connect(Server, Port, string.Empty);

            Redraw();
        }

        public void Disconnect()
        {
            TN3270.Close();
            Rtf = "";
        }

        public void Redraw()
        {
            RichTextBox Render = new RichTextBox();
            Render.Text = TN3270.CurrentScreenXML.Dump();

            Clear();
            Font fnt = new Font("Consolas", 10);
            Render.Font = new Font(fnt, FontStyle.Regular);

            IsRedrawing = true;
            Render.SelectAll();

            if (TN3270.CurrentScreenXML.Fields == null)
            {
                Color clr = Color.Lime;
                Render.SelectionProtected = false;
                Render.SelectionColor = clr;
                Render.DeselectAll();

                for (int i = 0; i < Render.Text.Length; i++)
                {
                    Render.Select(i, 1);
                    if (Render.SelectedText != " " && Render.SelectedText != "\n")
                        Render.SelectionColor = Color.Lime;
                }
                return;

                //Render.SelectionStart = (TN3270.CursorY) * 80 + TN3270.CursorX;
            }

            Render.SelectionProtected = true;
            foreach (Open3270.TN3270.XMLScreenField field in TN3270.CurrentScreenXML.Fields)
            {
                //if (string.IsNullOrEmpty(field.Text))
                //    continue;

                Application.DoEvents();
                Color clr = Color.Lime;
                if (field.Attributes.FieldType == "High" && field.Attributes.Protected)
                    clr = Color.White;
                else if (field.Attributes.FieldType == "High")
                    clr = Color.Red;
                else if (field.Attributes.Protected)
                    clr = Color.RoyalBlue;

                Render.Select(field.Location.position + field.Location.top, field.Location.length);
                Render.SelectionProtected = false;
                Render.SelectionColor = clr;
                if (clr == Color.White || clr == Color.RoyalBlue)
                    Render.SelectionProtected = true;
            }

            for (int i = 0; i < Render.Text.Length; i++)
            {
                Render.Select(i, 1);
                if (Render.SelectedText != " " && Render.SelectedText != "\n" && Render.SelectionColor == Color.Black)
                {
                    Render.SelectionProtected = false;
                    Render.SelectionColor = Color.Lime;
                }
            }

            Rtf = Render.Rtf;
           // TN3270.SetCursor(Coordinates.X, Coordinates.Y);

            IsRedrawing = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //The textbox eats several keystrokes, so we can't handle them from keybindings/commands.
            if (TN3270.IsConnected)
            {
                if (TerminalKeys.TerminalKeysDict.ContainsKey(e.KeyCode))
                {
                    TN3270.SendKey(true, TerminalKeys.TerminalKeysDict[e.KeyCode], 2000);
                }
                else
                {
                    //bool toUpperCase = false;

                    ////determine if casing is needed
                    //if (GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                    //    toUpperCase = false;
                    //else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                    //    toUpperCase = true;
                    //else if (GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                    //    toUpperCase = true;
                    //else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                    //    toUpperCase = false;

                    var buf = new StringBuilder(256);
                    var keyboardState = new byte[256];

                    //if (toUpperCase)
                    //    keyboardState[(int)Keys.ShiftKey] = 0xff;

                    GlobalHook.ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);
                    var selectedKey = buf.ToString().ToUpper();
                    TN3270.SetText(selectedKey);
                }
                Redraw();
            }
           //e.Handled = true;
                
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //if (e.KeyChar.)
            //if (e.KeyChar == '\r')
            //{
            //    TN3270.SendKey(true, TnKey.Enter, 1000);
            //    Redraw();
            //    e.Handled = true;
            //    return;
            //}
            //if (e.KeyChar == '\b')
            //    return;
            //if (e.KeyChar == '\t')
            //    return;

            //TN3270.SetText(e.KeyChar.ToString());
            e.Handled = true;
            //Redraw();
            base.OnKeyPress(e);
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            if (TN3270.IsConnected)
            {
                if (!IsRedrawing)
                {
                    int i = SelectionStart, x, y = 0;
                    while (i >= 81)
                    {
                        y++;
                        i -= 81;
                    }
                    x = i;

                    Coordinates = new Point(x, y);
                    TN3270.SetCursor(Coordinates.X, Coordinates.Y);
                    
                    base.OnSelectionChanged(e);
                }
            }
        }
    }
}
