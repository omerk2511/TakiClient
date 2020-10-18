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
                playerNum = 4;
            }
            playersList = new List<Player>();
            for (int i = 0; i < playerNum; i++)
            {
                playersList.Add(new Player());
            }
        }

        public Player GetPlayer(int playerIndex)
        {
            return playersList[playerIndex];
        }
    }
}
