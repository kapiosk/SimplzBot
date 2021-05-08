using System;
using System.Collections.Generic;
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
        private bool botFlag;
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
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private void HandleHotkey()
        {
            if (listBox1.Items.Count > 0)
            {
                if (listBox1.SelectedIndex == -1 && listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;

                if (!(listBox1.SelectedItem is null))
                {
                    SendKeys.Send(listBox1.SelectedItem.ToString());
                    if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                        listBox1.SelectedIndex += 1;
                }
            }
            else
            {
                botFlag = !botFlag;
                if (botFlag) timer1.Start();
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

        private int mouseMove = 50;
        private void timer1_Tick(object sender, EventArgs e)
        {
            mouseMove *= -1;
            var pos = MouseHandler.GetCursorPosition();
            pos.Y += mouseMove;
            MouseHandler.MoveCursorToPoint(pos.X, pos.Y);
            MouseHandler.DoMouseClick();
        }
    }
}
