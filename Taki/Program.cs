using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using Taki.Cards;

namespace Taki
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Game game = new Game(4);
                Player myplayer = game.GetPlayer(0);
                if (myplayer is ActivePlayer)
                {
                    Tuple<int, List<Color>> t = ((ActivePlayer)myplayer).GetCommonColor();
                    Console.WriteLine(t.Item1);
                    foreach (Color color in t.Item2)
                    {
                        Console.WriteLine(color);
                    }
                }
                
            }catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
