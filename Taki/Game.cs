using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class Game
    {
        private List<Player> playersList;
        public List<Card> usedCards;
        public string jwt;
        public int gameId;

        public Game(string activePlayerName, string[] playerNames, int gameId)
        {
            if (playerNames.Length > 3)  // TODO: check server's player list
            {
                throw new ArgumentException();
            }

            this.gameId = gameId;

            playersList = new List<Player>();
            playersList.Add(new ActivePlayer(activePlayerName));
            for (int i = 1; i < playerNames.Length; i++)
            {
                playersList.Add(new NonActivePlayer(playerNames[i], 8));
            }

            usedCards = new List<Card>();
        }

        public Player GetPlayer(int playerIndex)
        {
            return playersList[playerIndex];
        }

        public ActivePlayer GetActivePlayer()
        {
            return (ActivePlayer)playersList[0];
        }
        
        public int GetPlayersAmount()
        {
            return playersList.Count;
        }
        
        public int GetPlayerIndex(Player player)
        {
            for(int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].Equals(player))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
