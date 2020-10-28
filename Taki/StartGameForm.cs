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
    public partial class StartGameForm : Form
    {
        private Client client;
        private string name;
        private Thread handleNewMemebersThread;
        private bool execThread;
        private int playerNum;

        public StartGameForm(Client client, string gameID, string name)
        {
            InitializeComponent();
            this.client = client;
            this.gameIDLabel.Text += gameID;
            this.name = name;
            this.playerNum = 1;

            this.listBox1.Items.Add(this.name);
            handleNewMemebersThread = new Thread(HandleNewMembers);
            execThread = true;
            handleNewMemebersThread.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

       

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.execThread = false;
            handleNewMemebersThread.Join();
            StartGameJSON startGameJSON = new StartGameJSON(client.jwt);
            this.client.SendJSON(startGameJSON);
            dynamic statusJson = this.client.RecvJSON(true) ;
            if(statusJson.code == "game_starting")
            {
                List<JSONCard> activePlayerCards = new List<JSONCard>();
                foreach (dynamic jcard in statusJson.args.cards)
                {
                    Console.WriteLine(jcard.ToString());
                    JSONCard temp = JsonConvert.DeserializeObject<JSONCard>(jcard.ToString());
                    activePlayerCards.Add(temp);
                }
                string[] players = new string[4];

                for (int i = 0; i < 4; i++)
                {
                    players[i] = statusJson.args.players[i];
                }

                MethodInvoker methodInvokerDelegate = delegate ()
                {
                    Game game = new Game(players, activePlayerCards);
                    Form form = new GameWindow(game, this.client);
                    form.Location = this.Location;
                    form.StartPosition = FormStartPosition.Manual;
                    form.FormClosing += delegate { Environment.Exit(0); };
                    form.Show();
                    this.Hide();
                };
                this.Invoke(methodInvokerDelegate);
            }
            else if(statusJson.status == "denied")
            {
                MessageBox.Show(statusJson.args.message.ToString(), "Error");
                handleNewMemebersThread = new Thread(HandleNewMembers);
                this.execThread = true;
                handleNewMemebersThread.Start();
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

        private void EnableText(bool enable)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            { this.StartButton.Enabled = enable; };
            this.Invoke(methodInvokerDelegate);
        }

        private void HandleNewMembers()
        {
            try
            {
                while (this.execThread)
                {
                    dynamic json;
                    json = client.RecvJSON(false);
                    if(json == null)
                    {
                        continue;
                    }
                    if (json.code == "player_joined")
                    {
                        this.playerNum++;
                        AddListBoxItem(json.args.player_name.ToString());

                    }
                    if (json.code == "player_left")
                    {
                        this.playerNum--;
                        RemoveListBoxItem(json.args.player_name.ToString());
                    }
                    SetLabelText("Members: " + this.playerNum.ToString() + "/4");
                    if (this.playerNum == 4)
                        EnableText(true);
                    else
                        EnableText(false);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void StartGameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
