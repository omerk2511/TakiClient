using LoadingIndicator.WinForms;
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
        
        public StartGameForm(Client client, string gameID)
        {
            InitializeComponent();
            this.client = client;
            this.gameIDLabel.Text += gameID;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void StartGameForm_Load(object sender, EventArgs e)
        {

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
    }
}
