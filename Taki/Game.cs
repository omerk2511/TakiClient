using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    class Game
    {
        private List<Player> playersList;
        public List<ColorCard> usedCards;

        public bool IsTwoPlusActive { get; set; }
        public Color CurrentColor { get; set; }

        public Game(string[] playerNames, List<JSONCard> activePlayerCards)
        {
            if (playerNames.Length != 4) 
            {
                throw new ArgumentException();
            }
            playersList = new List<Player>();
            playersList.Add(new ActivePlayer(playerNames[0], activePlayerCards));
            for (int i = 1; i < playerNames.Length; i++)
            {
                playersList.Add(new NonActivePlayer(playerNames[i], 8));
            }

            usedCards = new List<ColorCard>();
            CurrentColor = Color.UNDEFINED;
            IsTwoPlusActive = false;
        }

        public Player GetPlayer(int playerIndex)
        {
            return playersList[playerIndex];
        }

        public Player GetPlayerByName(string playerName)
        {
            return this.playersList.Find(p => p.name == playerName);
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
            return this.playersList.FindIndex(p => p == player);
        }
       
    }
}
