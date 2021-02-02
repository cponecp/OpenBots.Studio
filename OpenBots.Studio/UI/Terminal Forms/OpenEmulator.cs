using Open3270;
using OpenBots.Commands.Terminal.Library;
using OpenBots.Utilities;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Forms
{
    public class OpenEmulator : RichTextBox, IAudit
    {
        public TNEmulator TN3270 = new TNEmulator();
        private bool IsRedrawing = false;
        public Point Coordinates { get; set; } = new Point(0, 0);
        public int FieldIndex { get; set; } = 0;

        public void Connect(string Server, int Port, string Type, bool UseSsl)
        {
            try
            {
                TN3270.Config.UseSSL = UseSsl;
                TN3270.Config.TermType = Type;
                TN3270.Audit = this;
                //TN3270.Debug = true;
                TN3270.Config.FastScreenMode = true;

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

                Text = TN3270.CurrentScreenXML.Dump();

                SelectAll();
                SelectionColor = Color.Lime;

                if (TN3270.CurrentScreenXML.Fields == null)
                {
                    SelectionProtected = false;
                    SelectionColor = Color.Lime;
                    DeselectAll();

                    for (int i = 0; i < Text.Length; i++)
                    {
                        Select(i, 1);
                        if (SelectedText != " " && SelectedText != "\n")
                            SelectionColor = Color.Lime;
                    }
                    return;
                }

                SelectionProtected = true;
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

                    Select((field.Location.top * 84 + 172) + field.Location.left + 3, field.Location.length);
                    SelectionProtected = false;
                    SelectionColor = clr;
                    if (clr == Color.White || clr == Color.RoyalBlue)
                        SelectionProtected = true;
                }

                Console.WriteLine("Before " + Coordinates.ToString());

                IsRedrawing = false;

                Select((TN3270.CursorY * 84 + 172) + TN3270.CursorX + 3, 0);
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
                    var fields = TN3270.GetFields();
                    Console.WriteLine(fields);

                    FieldIndex = -1;

                    foreach (Open3270.TN3270.XMLScreenField field in TN3270.CurrentScreenXML.Fields)
                    { 
                        //if ((Coordinates.X >= field.Location.left && Coordinates.X < field.Location.left + field.Location.length) &&
                        //    (Coordinates.Y >= field.Location.top))// && Coordinates.Y <= field.Location))
                        //{
                        //    //FieldIndex = Array.IndexOf(TN3270.CurrentScreenXML.Fields, field);
                        //    //Console.WriteLine($"Field Index: {FieldIndex}");

                        //    Console.WriteLine(field);
                        //}
                        if ((Coordinates.Y * 80 + Coordinates.X) >= field.Location.position &&
                            (Coordinates.Y * 80 + Coordinates.X) < field.Location.position + field.Location.length)
                        {
                            FieldIndex = Array.IndexOf(TN3270.CurrentScreenXML.Fields, field);
                            Console.WriteLine($"Field index = {FieldIndex}");
                            Console.WriteLine(field.Text);
                            
                        }
                    }

                    base.OnSelectionChanged(e);
                }
            }
        }

        public void InitializeComponent()
        {
            SuspendLayout();
            // 
            // OpenEmulator
            // 
            Font = new Font("Consolas", 10F, FontStyle.Regular);
            Size = new Size(Font.Height * 39, Font.Height * 31);
            ResumeLayout(false);

        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}
