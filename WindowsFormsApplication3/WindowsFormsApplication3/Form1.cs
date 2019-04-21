using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        String auth_key = "";
        Form2 myform = new Form2();
        ArrayList tokens = new ArrayList();
        int sentCount = 0, failedCount = 0;
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if(Clipboard.GetText().Length > 0)
            {
                textBox1.Text = Clipboard.GetText();
                Clipboard.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            myform.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            label9.Text = "";
            sentCount = 0;
            failedCount = 0;
            if (File.Exists("config.ini"))
            {
                using (StreamReader sr = File.OpenText("config.ini"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        auth_key = s.Trim();
                        break;
                    }
                }
                if (auth_key == "")
                {
                    MessageBox.Show("Please open API Setting and set the authorization key first!", "API key not set!");
                }
                else
                {
                    sendNotification();
                }
            }
            else
            {
                MessageBox.Show("Please open API Setting and set the authorization key first!", "API key not set!");
            }
        }

        private void sendNotification()
        {
            using (var client = new WebClient())
            {
                bool error = false;
                if(textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Token not set!");
                    error = true;
                }
                if (textBox2.Text.Trim() == "")
                {
                    MessageBox.Show("Title not set!");
                    error = true;
                }
                if (textBox3.Text.Trim() == "")
                {
                    MessageBox.Show("Message Body not set!");
                    error = true;
                }
                if (!error)
                {
                    client.Headers["Content-Type"] = "application/json";
                    client.Headers["Authorization"] = "key=" + auth_key;
                    if(tokens.Count > 0)
                    {
                        foreach (String token in tokens)
                        {
                            var payload = new
                            {
                                to = token,
                                notification = new
                                {
                                    body = textBox3.Text,
                                    title = textBox2.Text,
                                },
                                data = new
                                {
                                    message = "nothing"
                                },
                                android = new
                                {
                                    ttl = "36500s"
                                }
                            };

                            var jsonBody = JsonConvert.SerializeObject(payload);
                            try
                            {
                                String reply = client.UploadString("https://fcm.googleapis.com/fcm/send", "POST", jsonBody);
                                if (reply.Contains("\"success\":1"))
                                {
                                    label8.Text = "Sent: " + (sentCount + 1);
                                    sentCount++;
                                    label9.Text = "Failed: " + failedCount;
                                }
                                else
                                {
                                    label8.Text = "Sent: " + sentCount;
                                    label9.Text = "Failed: " + (failedCount + 1);
                                    failedCount++;
                                }
                            }
                            catch(Exception)
                            {
                                label8.Text = "Sent: " + sentCount;
                                label9.Text = "Failed: " + (failedCount + 1);
                                failedCount++;
                            }
                            
                        }
                        
                    }
                    else
                    {
                        var payload = new
                        {
                            to = textBox1.Text,
                            notification = new
                            {
                                body = textBox3.Text,
                                title = textBox2.Text,
                            },
                            data = new
                            {
                                message = "nothing"
                            }
                        };

                        var jsonBody = JsonConvert.SerializeObject(payload);
                        String reply = client.UploadString("https://fcm.googleapis.com/fcm/send", "POST", jsonBody);
                        if (reply.Contains("\"success\":1"))
                        {
                            label8.Text = "Sent: " + sentCount++;
                        }
                        else
                        {
                            label9.Text = "Failed: " + failedCount++;
                        }
                    }
                    
                }
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if(res.ToString() == "Cancel")
            {
                label7.Text = "";
                tokens.Clear();
                textBox1.Text = "";
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            openFileDialog1.Multiselect = false;
            String path = openFileDialog1.FileName;
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    tokens.Add(s);
                }
            }
            if(tokens.Count > 0)
            {
                String tmp = "Loaded: " + Convert.ToString(tokens.Count);
                label7.Text = tmp;
                textBox1.Text = path;
            }
        }
    }
}
