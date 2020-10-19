using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    class ActivePlayer : Player
    {
        private List<Card> hand;

        public ActivePlayer()
        {
            hand = new List<Card>();
        }
        
        public ActivePlayer(List<JSONCard> jsonCards)
        {
            hand = new List<Card>();
            foreach(JSONCard c in jsonCards)
            {
                this.AddJSONCard(c);
            }
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        void AddJSONCard(JSONCard jsonCard)
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
            hand.Add(c);
        }

        public Card GetResourcesNames(int cardIndex)
        {
            return hand[cardIndex];
        }
        
        public Card GetCard(int cardIndex)
        {
            return hand[cardIndex];
        }

        public override int GetCardAmount()
        {
            return hand.Count;
        }

        // Remove the card specified by cardIndex and return it
        public Card RemoveCard(int cardIndex)
        {
            Card tempCard = hand[cardIndex];
            hand.RemoveAt(cardIndex);
            return tempCard;
        }

        public override List<string> GetCardResources()
        {
            List<string> lst = new List<string>(this.hand.Count);
            foreach (Card c in this.hand)
            {
                lst.Add(c.GetResourceName());
            }
            return lst;
        }
    }
}
