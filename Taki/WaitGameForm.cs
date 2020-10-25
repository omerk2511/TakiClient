using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taki
{
    public partial class WaitGameForm : Form
    {
        private Client client;
        private string[] names;
        public WaitGameForm(Client client,string[] names)
        {
            InitializeComponent();
            this.client = client;
            this.names = names;
        }

        private void WaitGameForm_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
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
            int num = 0;
            foreach (string member in names)
            {
                if (member != null)
                {
                    AddListBoxItem(member);
                    num++;
                }
            }

            try
            {   
                while (true)
                {
                    SetLabelText("Members: " + num.ToString() + "/4");
                    dynamic json = client.RecvJSON();
                    Console.WriteLine(json);
                    if (json.code == "player_joined")
                    {
                        num++;
                        AddListBoxItem(json.args.player_name.ToString());
                    }
                    else if (json.code == "player_left")
                    {
                        num--;
                        RemoveListBoxItem(json.args.player_name.ToString());
                    }
                    else if (json.code == "game_ended")
                    {
                        MessageBox.Show("The game was ended by the host", "Error");
                        Application.Exit();
                    }
                        
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}
