using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taki
{
    class CardControl : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Console.WriteLine("LOLXD");
            e.Graphics.RotateTransform(100f);
            
        }
    }
}
