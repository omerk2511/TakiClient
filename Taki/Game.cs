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
            playersList.Add(new ActivePlayer());
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
