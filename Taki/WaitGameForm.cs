using Newtonsoft.Json;
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
    public partial class WaitGameForm : Form
    {
        private Client client;
        private Thread handleConnectionThread;
        private bool execThread;
        private int playerNum;

        public WaitGameForm(Client client, List<string> names)
        {
            InitializeComponent();
            this.client = client;
            this.playerNum = names.Count;
            this.listBox1.Items.AddRange(names.ToArray());
            handleConnectionThread = new Thread(HandleConnection);
            execThread = true;
            handleConnectionThread.Start();
        }

        private void WaitGameForm_Load(object sender, EventArgs e)
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

        private void HandleConnection()
        {
            try
            {
                while (this.execThread)
                {
                    SetLabelText("Members: " + this.playerNum.ToString() + "/4");
                    dynamic json = client.RecvJSON(true);
                    if (json.code == "player_joined")
                    {
                        this.playerNum++;
                        AddListBoxItem(json.args.player_name.ToString());
                    }
                    else if (json.code == "player_left")
                    {
                        this.playerNum--;
                        RemoveListBoxItem(json.args.player_name.ToString());
                    }
                    else if (json.code == "game_ended")
                    {
                        MessageBox.Show("The game was ended by the host", "Error");
                        Application.Exit();
                    }
                    else if (json.code == "game_starting")
                    {
                        this.execThread = false;
                        List<JSONCard> activePlayerCards = new List<JSONCard>();
                        foreach (dynamic jcard in json.args.cards)
                        {
                            JSONCard temp = JsonConvert.DeserializeObject<JSONCard>(jcard.ToString());
                            activePlayerCards.Add(temp);
                        }
                        string[] players = new string[4];

                        for (int i = 0; i < 4; i++)
                        {
                            players[i] = json.args.players[i];
                        }

                        Game game = new Game(players, activePlayerCards);
                        Form form = new GameWindow(game, this.client);
                        form.Location = this.Location;
                        form.StartPosition = FormStartPosition.Manual;
                        form.FormClosing += delegate { Environment.Exit(0); };
                        form.Show();
                        this.Hide();
                    }

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }
    }
}
