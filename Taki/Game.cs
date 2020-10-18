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
        public List<Card> usedCards;

        // playersNum must be a number between 2 to 4
        public Game(int playerNum)
        {
            if (playerNum > 4 || playerNum < 2)
            {
                throw new ArgumentException();
            }
            playersList = new List<Player>();
            playersList.Add(new ActivePlayer());
            for (int i = 1; i < playerNum; i++)
            {
                playersList.Add(new NonActivePlayer(8));
            }

            usedCards = new List<Card>();
            for (int i = 0; i < 50; i++)
            {
                usedCards.Add(new NumberCard(1, Color.YELLOW));
            }
        }

        public Player GetPlayer(int playerIndex)
        {
            return playersList[playerIndex];
        }
    }
}
