using Newtonsoft.Json.Linq;
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
        private Game game;

        public ActivePlayer(string name, Game game2) : base(name)
        {
            hand = new List<Card>();
            game = game2;
        }
        
        public ActivePlayer(string name, List<JSONCard> jsonCards,Game game2) : base(name)
        {
            hand = new List<Card>();
            foreach(JSONCard c in jsonCards)
            {
                this.AddJSONCard(c);
            }
            game = game2;
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        Card AddJSONCard(JSONCard jsonCard)
        {
            Card c = null;
            game.ConvertJsonCardToCard(jsonCard);
            hand.Add(c);
            return c;
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

        public List<Card> PlayCard(List<int> cardIndexes, Client client)
        {
            List<Card> cards = new List<Card>();
            JSONCard[] jsonCards = new JSONCard[cardIndexes.Count];
            int place = 0;
            foreach (int index in cardIndexes)
            {
                Card card = RemoveCard(index);
                game.usedCards.Add(card);
                JSONCard jsonCard = card.Serialize();
                jsonCards[place] = jsonCard;
                place++;
                cards.Add(card);
            }
            PlayCardJSON doMoveJson = new PlayCardJSON(client.jwt, jsonCards);
            client.SendJSON(doMoveJson);
            return cards;

        }
        
        public List<Card> DrawCard(Client client)
        {
            TakeCardsJSON doMoveJson = new TakeCardsJSON(client.jwt);
            client.SendJSON(doMoveJson);

            dynamic cardsTakenJSON;
            while (true)
            {
                cardsTakenJSON = client.RecvJSON(true);
                if (cardsTakenJSON.status == "success")
                {
                    if (((JToken)cardsTakenJSON)["args"].Type != JTokenType.Null)
                    {
                        break;
                    }
                }
            }


            List<JSONCard> jsonCardsTaken = ((JArray)cardsTakenJSON.args.cards).ToObject<List<JSONCard>>();
            List<Card> cardsTaken = new List<Card>();
            foreach(JSONCard card in jsonCardsTaken)
            {
                cardsTaken.Add(this.AddJSONCard(card));
            }
            return cardsTaken;
        }

        public int GetCardAmountOfColor(Color color)
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if  (card is ColorCard)
                {
                    if (((ColorCard)card).Color == color)
                        counter++;
                }
            }
            return counter;
        }
        
        public Dictionary<Color, int> GetColorAmountDictionary()
        {
            Dictionary<Color, int> dic = new Dictionary<Color, int>(); 
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    dic[((ColorCard)card).Color]++;
                }

            }
            return dic;
        }

        public Tuple<int, List<Color>> GetCommonColor()
        {
            ///Return tuple with the common colors and how many of them.
            
            int max = 0;
            int num = 0;
            List <Color> lst = new List<Color>();
            Color[] allColors = new Color[4] {Color.BLUE, Color.RED, Color.GREEN, Color.YELLOW };
            for (int i = 0; i < 4; i++)
            {
                num = GetCardAmountOfColor(allColors[i]);
                if (num > max)
                {
                    max = num;
                    lst.Clear();
                    lst.Add(allColors[i]);
                }
                else
                {
                    if (num == max)
                    {
                        lst.Add(allColors[i]);
                    }
                }

            }
            return new Tuple<int, List<Color>>(max, lst);
        }

        public List<Card> GetAllCardsOfColor(Color color)
        {
            List<Card> lst = new List<Card>();
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    if (((ColorCard)card).Color == color)
                    {
                        lst.Add(card);
                    }
                }
            }
            return lst;
        }

        public List<Card> GetCardWithTypeOrNumber(Card card2)
        {
            List<Card> lst = new List<Card>();
            foreach (Card card in hand)
            {
                if (card.GetType().Equals(card2.GetType()))
                {
                    if (card is NumberCard)
                    {
                        if (((NumberCard)card).Number == ((NumberCard)card2).Number)
                            lst.Add(card);
                    }
                    else
                        lst.Add(card);
                }
            }
            return lst;
        }

        public int GetActionCardsAmount(Color color)
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    if (((ColorCard)card).Color== color)
                    {
                        if (!(card is NumberCard))
                            counter++;
                    }          
                }
            }
            return counter;
        }

        public int CardAmountOfType(Type t)
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card.GetType().Equals(t))
                {
                    counter++;
                }
            }
            return counter;     
        }
    }
}
