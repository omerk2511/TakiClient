using Newtonsoft.Json.Linq;
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
    public partial class JoinGameForm : Form
    {
        private string playerName;
        public JoinGameForm(string playerName)
        {
            this.playerName = playerName;
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            string playerName = this.playerName;
            string gameID = this.GameIDInput.Text;
            string password = this.PasswordInput.Text;
            if (password == "" || gameID == "")
            {
                this.ErrorLabel.Text = "One of the values you entered was empty.";
            }else if(!int.TryParse(gameID, out _))
            {
                this.ErrorLabel.Text = "Game ID must be a number from 0 to 10000";
            }
            else
            {
                this.ErrorLabel.Text = "";
                try
                {
                    JoinGameJSON json = new JoinGameJSON(gameID, playerName, password);
                    Client client = new Client();
                    client.SendJSON(json);
                    dynamic jsonObj = client.RecvJSON(true);
                    if(jsonObj.status == "success")
                    {
                        client.jwt = jsonObj.args.jwt;

                        // Join the game with server
                        Form form = new WaitGameForm(client, ((JArray)jsonObj.args.players).ToObject<List<string>>());
                        form.FormClosing += delegate { Environment.Exit(0); };
                        form.Show();
                        this.Hide();
                    }
                    else
                    {
                        Console.WriteLine(jsonObj.ToString());
                    }
                    
                }
                catch (TakiServerException exception)
                {
                    this.ErrorLabel.Text = exception.Message;
                }
            }
        }
    }
}
