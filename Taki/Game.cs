using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class Game
    {
        private List<Player> playersList;

        // playersNum must be a number between 2 to 4
        public Game(int playerNum)
        {
            if (playerNum > 4 || playerNum < 2)
            {
                throw new ArgumentException();
            }
            playersList = new List<Player>();
            JSONCard card1 = new JSONCard("number_card", "green", "3");
            JSONCard card2 = new JSONCard("number_card", "green", "3");
            JSONCard card3 = new JSONCard("number_card", "blue", "3");
            JSONCard card4 = new JSONCard("number_card", "blue", "3");
            JSONCard card5 = new JSONCard("number_card", "blue", "3");
            JSONCard card6 = new JSONCard("stop", "green", "5");
            JSONCard card7 = new JSONCard("number_card", "green", "4");
            JSONCard card8 = new JSONCard("number_card", "blue", "3");
            JSONCard[] arr = new JSONCard[8] { card1, card2, card3, card4, card5, card6, card7, card8 };
            playersList.Add(new ActivePlayer(arr));
            for (int i = 1; i < playerNum; i++)
            {
                playersList.Add(new NonActivePlayer(8));
            }
        }

        public Player GetPlayer(int playerIndex)
        {
            return playersList[playerIndex];
        }

    }
}
