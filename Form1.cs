using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplzBot
{
    public partial class Form1 : Form
    {
        private readonly KeyHandler ghk;
        public Form1()
        {
            InitializeComponent();
            ghk = new KeyHandler(Keys.F8, this);
            ghk.Register();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            if (!(listBox1.SelectedItem is null))
            {
                SendKeys.Send(listBox1.SelectedItem.ToString());
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                    listBox1.SelectedIndex += 1;
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
            if (!(listBox1.SelectedItem is null))
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
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
    }
}
