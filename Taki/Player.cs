using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    class Player
    {
        private List<Card> hand;

        public Player() {
            hand = new List<Card>();
        }

        void AddCard(Card card)
        {
            hand.Add(card);
        }
        
        void AddJSONCard(JSONCard jsonCard)
        {
            Card c = null;
            string cardValue = jsonCard.value;
            Color cardColor = Color.GREEN;
            if (jsonCard.color != "")
            {
                cardColor = (Color)Enum.Parse(typeof(Color), jsonCard.color, true);
            }
            switch (jsonCard.type)
            {
                case ("number_card"):
                    c = new NumberCard(int.Parse(cardValue), cardColor);
                    break;
                case ("plus"):
                    c = new PlusCard(cardColor);
                    break;
                case ("plus_2"):
                    c = new TwoPlusCard(cardColor);
                    break;
                case ("stop"):
                    c = new StopCard(cardColor);
                    break;
                case ("change_direction"):
                    c = new ChangeDirectionCard(cardColor);
                    break;
                case ("change_color"):
                    c = new ChangeColorCard();
                    break;
                case ("taki"):
                    c = new TakiCard(cardColor);
                    break;
                case ("super_taki"):
                    c = new SuperTakiCard();
                    break;
            }
            hand.Add(c);
        }

        public Card GetCard(int cardIndex)
        {
            return hand[cardIndex];
        }
        
        public int GetCardAmount()
        {
            return hand.Count;
        }

        // Remove the card specified by cardIndex and return it
        public Card PlayCard(int cardIndex)
        {
            Card tempCard = hand[cardIndex];
            hand.RemoveAt(cardIndex);
            return tempCard;
        }
    }
}
