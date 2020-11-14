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
        private readonly List<Card> hand;

        public ActivePlayer(string name) : base(name)
        {
            hand = new List<Card>();
        }
        
        public ActivePlayer(string name, List<JSONCard> jsonCards) : base(name)
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

        public Card ConvertJsonCardToCard(JSONCard jsonCard)
        {
            Card c = null;
            dynamic cardValue = jsonCard.value;
            Color cardColor = Color.UNDEFINED;
            if (jsonCard.color != "")
            {
                cardColor = (Color)Enum.Parse(typeof(Color), jsonCard.color, true);
            }
            switch (jsonCard.type)
            {
                case "number_card":
                    c = new NumberCard(Convert.ToInt32(cardValue), cardColor);
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

        Card AddJSONCard(JSONCard jsonCard)
        {
            Card c = ConvertJsonCardToCard(jsonCard);
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
        public void RemoveCard(Card card)
        {
            hand.Remove(card);
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

        public void PlayCard(List<Card> cardsToPlay, Client client, Color specialColor)
        {
            JSONCard[] jsonCards = new JSONCard[cardsToPlay.Count];
            for (int i = 0; i < jsonCards.Length; i++)
            {
                JSONCard jsonCard = (cardsToPlay[i] is ISpecialCard card) ? card.SerializeColor(specialColor) : cardsToPlay[i].Serialize();
                jsonCards[i] = jsonCard;
                hand.Remove(cardsToPlay[i]);
            }
            PlayCardJSON doMoveJson = new PlayCardJSON(client.jwt, jsonCards);
            client.SendJSON(doMoveJson);
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
                if (card.GetType().Equals(card2.GetType()) && card2 != card)
                {
                    if (card is NumberCard numberCard)
                    {
                        if (numberCard.Number == ((NumberCard)card2).Number)
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
                if (card is ColorCard colorCard)
                {
                    if (colorCard.Color == color)
                    {
                        if (!(card is NumberCard))
                            counter++;
                    }          
                }
            }
            return counter;
        }

        public T GetCardOfType<T>() where T : Card
        {
            foreach (Card card in hand)
            {
                if (card is T c)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
