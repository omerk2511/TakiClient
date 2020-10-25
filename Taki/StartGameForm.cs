using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taki
{
    public partial class StartGameForm : Form
    {
        private Client client;
        private string name;

        public StartGameForm(Client client, string gameID,string name)
        {
            InitializeComponent();
            this.client = client;
            this.gameIDLabel.Text += gameID;
            this.name = name;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void StartGameForm_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartGameJSON startGameJSON = new StartGameJSON(client.jwt);
            this.client.SendJSON(startGameJSON);
            dynamic json = this.client.RecvJSON();
            if(json.status == "success")
            {
                Console.WriteLine(json.ToString());
            }
            else if(json.status == "denied")
            {
                MessageBox.Show(json.args.message.ToString(), "Error");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void gameIDLabel_Click(object sender, EventArgs e)
        {

        }
        private void AddListBoxItem(string item)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            { this.listBox1.Items.Add(item); };
            this.Invoke(methodInvokerDelegate);
        }
        private void RemoveListBoxItem(string item)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            { this.listBox1.Items.Remove(item); };
            this.Invoke(methodInvokerDelegate);
        }
        private void SetLabelText(string text)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            { this.memeberLabel.Text = text; };
            this.Invoke(methodInvokerDelegate);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AddListBoxItem(this.name);
                int num = 1;
                while (true)
                {
                    dynamic json = client.RecvJSON();
                    if (json.code == "player_joined")
                    {
                        num++;
                        AddListBoxItem(json.args.player_name.ToString());

                    }
                    if (json.code == "player_left")
                    {
                        num--;
                        RemoveListBoxItem(json.args.player_name.ToString());
                    }
                    Console.WriteLine(json);
                    SetLabelText("Members: " + num.ToString() + "/4");
                    if (num == 4)
                        StartButton.Enabled = true;
                    else
                        StartButton.Enabled = false;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
           
        }
    }
}
