using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SimplzBot
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, Keys> KeyBindings = new()
        {
            { "F8", Keys.F8 },
            { "F9", Keys.F9 },
        };

        private KeyHandler ghk;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(ghk is null))
                ghk.Unregiser();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var kb in KeyBindings.Keys)
                comboBox1.Items.Add(kb);
            comboBox1.SelectedIndex = 0;

            listBox1.Items.Add("Name{tab}{tab}Surname");
            listBox1.Items.Add("Id");
            listBox1.Items.Add("DoB dd/MM/yyyy");
            listBox1.Items.Add("Mobile");
            listBox1.Items.Add("Confirmation mobile");
            listBox1.Items.Add("e-mail");
            listBox1.Items.Add("Mouse,Move,100,100");
            listBox1.Items.Add("Mouse,RightClick");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private void HandleHotkey(bool fromBot = false)
        {
            if (getMousePos)
            {
                GetMousePos();
                ResetButtonColor(btnMouse);
            }
            else if (fromBot || numericUpDown1.Value == 0)
            {
                if (listBox1.SelectedIndex == -1 && listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;

                if (!(listBox1.SelectedItem is null))
                {
                    var com = listBox1.SelectedItem.ToString();
                    var comParts = com.Split(",");
                    if (comParts[0].Equals("Mouse") && comParts.Length > 1)
                    {
                        switch (comParts[1])
                        {
                            case "Move":
                                if (comParts.Length == 4 && int.TryParse(comParts[2], out int x) && int.TryParse(comParts[3], out int y))
                                    MouseHandler.MoveCursorToPoint(x, y);
                                break;
                            case "LeftClick":
                                MouseHandler.DoLeftMouseClick();
                                break;
                            case "RightClick":
                                MouseHandler.DoRightMouseClick();
                                break;
                        }
                    }
                    else if (comParts[0].Equals("Timer") && comParts.Length > 1)
                    {
                        timer1.Interval = int.Parse(comParts[1]);
                    }
                    else
                    {
                        SendKeys.Send(listBox1.SelectedItem.ToString());
                    }
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                        listBox1.SelectedIndex += 1;
                    else
                        listBox1.SelectedIndex = 0;
                }
            }
            else if (numericUpDown1.Value > 0)
            {
                timer1.Interval = (int)numericUpDown1.Value;
                if (!timer1.Enabled) timer1.Start();
                else timer1.Stop();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                listBox1.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is null))
            {
                Clipboard.SetText(listBox1.SelectedItem.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is null))
            {
                if (KeyBindings.TryGetValue(comboBox1.SelectedItem.ToString(), out var kb))
                {
                    if (!(ghk is null))
                        ghk.Unregiser();

                    ghk = new KeyHandler(kb, this);
                    ghk.Register();
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
            {
                MoveItem(listBox1.SelectedIndex, listBox1.SelectedIndex - 1);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1 && listBox1.SelectedIndex < listBox1.Items.Count - 1)
            {
                MoveItem(listBox1.SelectedIndex, listBox1.SelectedIndex + 1);
            }
        }

        private void MoveItem(int indxFrom, int indxFromTo)
        {
            var tmp = listBox1.Items[indxFrom];
            listBox1.Items.RemoveAt(indxFrom);
            listBox1.Items.Insert(indxFromTo, tmp);
            listBox1.SelectedIndex = indxFromTo;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            HandleHotkey(true);
        }

        private bool getMousePos = false;
        private void btnMouse_Click(object sender, EventArgs e)
        {
            getMousePos = !getMousePos;
            if (getMousePos)
                btnMouse.BackColor = Color.Red;
            else
                ResetButtonColor(btnMouse);
        }

        private void GetMousePos()
        {
            var pos = MouseHandler.GetCursorPosition();
            listBox1.Items.Add($"Mouse,Move,{pos.X},{pos.Y}");
            getMousePos = false;
        }

        private static void ResetButtonColor(Button btn)
        {
            btn.BackColor = DefaultBackColor;
            btn.UseVisualStyleBackColor = true;
        }
    }
}
