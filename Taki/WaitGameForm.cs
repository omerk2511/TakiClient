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
        public WaitGameForm(Client client)
        {
            InitializeComponent();
            this.client = client;
        }
    }
}
