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
            }
            else
            {
                this.ErrorLabel.Text = "";
                try
                {
                    // Join the game with server
                    Game game = new Game(4); //TODO: Replace this with a function that creates a game
                    Form form = new GameWindow(game);
                    form.FormClosing += delegate { Environment.Exit(0); };
                    form.Show();
                    this.Hide();
                }
                catch (TakiServerException exception)
                {
                    this.ErrorLabel.Text = exception.Message;
                }
            }
        }
    }
}
