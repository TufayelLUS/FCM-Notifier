using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your Firebase App Authorization Key can be found in your project settings and then going to cloud messaging.\nMake sure you have successfully implemented notification listener properly.", "What is this?");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 myform = new Form1();
            myform.Show();
            this.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (File.Exists("config.ini"))
            {
                using (StreamReader sr = File.OpenText("config.ini"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        textBox1.Text = s.Trim();
                        break;
                    }
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 myform = new Form1();
            myform.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String auth_key = textBox1.Text;
            if (File.Exists("config.ini"))
            {
                File.Delete("config.ini");
            }
            using (FileStream fs = File.Create("config.ini"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(auth_key);
                fs.Write(info, 0, info.Length);
            }
            Form1 myform = new Form1();
            myform.Show();
            this.Hide();
        }
    }
}
