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
            try
            {
                TN3270.Config.UseSSL = UseSsl;
                TN3270.Config.TermType = Type;
                TN3270.Connect(Server, Port, string.Empty);

                Redraw();
            }
            catch (Exception)
            {

            }
            
        }

        public void Disconnect()
        {
            TN3270.Close();
            Rtf = "";
        }


        public delegate void RedrawDelegate();
        public void Redraw()
        {
            if (InvokeRequired)
            {
                var d = new RedrawDelegate(Redraw);
                Invoke(d, new object[] { });
            }
            else
            {
                IsRedrawing = true;

                RichTextBox Render = new RichTextBox();
                Render.Text = TN3270.CurrentScreenXML.Dump();

                Clear();
                Font fnt = new Font("Consolas", 10);
                Render.Font = new Font(fnt, FontStyle.Regular);


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
                Console.WriteLine("Before " + Coordinates.ToString());
                //Coordinates = new Point(TN3270.CursorX, TN3270.CursorY);



                IsRedrawing = false;

                this.Select((TN3270.CursorY * 84 + 172) + TN3270.CursorX + 3, 0);
                Console.WriteLine("After " + Coordinates.ToString());
            }
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (TN3270.IsConnected)
            {
                if (TerminalKeys.TerminalKeysDict.ContainsKey(e.KeyCode))
                {
                    TnKey sendKey = TerminalKeys.TerminalKeysDict[e.KeyCode];
                    TN3270.SendKey(false, sendKey, 2000);
                }
                else if (e.KeyCode == Keys.ShiftKey)
                    return;
                else
                {
                    bool toUpperCase = false;

                    //determine if casing is needed
                    if (GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                        toUpperCase = false;
                    else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                        toUpperCase = true;
                    else if (GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                        toUpperCase = true;
                    else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                        toUpperCase = false;

                    var buf = new StringBuilder(256);
                    var keyboardState = new byte[256];

                    if (toUpperCase)
                        keyboardState[(int)Keys.ShiftKey] = 0xff;

                    GlobalHook.ToUnicode((uint)e.KeyCode, 0, keyboardState, buf, 256, 0);
                    var selectedKey = buf.ToString();
                    TN3270.SetText(selectedKey);
                }
                Redraw();
            }
           e.Handled = true;
                
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
                    int i = SelectionStart -172 , x, y = 0;
                    while (i >= 84)
                    {
                        y++;
                        i -= 84;
                    }
                    x = i - 3;

                    Coordinates = new Point(x, y);
                    TN3270.SetCursor(Coordinates.X, Coordinates.Y);
                    
                    base.OnSelectionChanged(e);
                }
            }
        }
    }
}
