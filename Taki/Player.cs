using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    abstract class Player
    {
        public string name;

        public abstract int GetCardAmount();

        // The function will return a list of resource names for the cards
        // in the player's hand.
        public abstract List<string> GetCardResources();

        public Player(string name)
        {
            this.name = name;
        }

    }
}
