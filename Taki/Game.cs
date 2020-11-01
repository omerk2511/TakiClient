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
        public List<Card> usedCards;

        public Game(string[] playerNames, List<JSONCard> activePlayerCards)
        {
            if (playerNames.Length != 4) 
            {
                throw new ArgumentException();
            }
            playersList = new List<Player>();
            playersList.Add(new ActivePlayer(playerNames[0], activePlayerCards,this));
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
        public Card ConvertJsonCardToCard(JSONCard jsonCard)
        {
            Card c = null;
            string cardValue = jsonCard.value;
            Color cardColor = Color.UNDEFINED;
            if (jsonCard.color != "")
            {
                cardColor = (Color)Enum.Parse(typeof(Color), jsonCard.color, true);
            }
            switch (jsonCard.type)
            {
                case "number_card":
                    c = new NumberCard(int.Parse(cardValue), cardColor);
                    break;
                case "plus":
                    c = new PlusCard(cardColor);
                    break;
                case "plus_2":
                    c = new TwoPlusCard(cardColor);
                    break;
                case "stop":
                    c = new StopCard(cardColor);
                    break;
                case "change_direction":
                    c = new ChangeDirectionCard(cardColor);
                    break;
                case "change_color":
                    c = new ChangeColorCard();
                    break;
                case "taki":
                    c = new TakiCard(cardColor);
                    break;
                case "super_taki":
                    c = new SuperTakiCard();
                    break;
               
            }
            return c;
        }
    }
}
