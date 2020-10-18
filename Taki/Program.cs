﻿using System;
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
