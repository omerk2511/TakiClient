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
        
        public ActivePlayer(JSONCard[] jsonCards)
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

        public Card GetCard(int cardIndex)
        {
            return hand[cardIndex];
        }

        public int GetCardAmount()
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

        public JSONCard PlayCard(int cardIndex)
        {
            Card card = RemoveCard(cardIndex);
            JSONCard json = new JSONCard("plus_2", ((ColorCard)card).Color.ToString().ToLower(), "");
            if (card is NumberCard)
                json = new JSONCard("number_card", ((ColorCard)card).Color.ToString().ToLower(), ((NumberCard)card).Number.ToString());
            if (card is PlusCard)
                json = new JSONCard("plus", ((ColorCard)card).Color.ToString().ToLower(),"");
            if (card is StopCard)
                json = new JSONCard("stop", ((ColorCard)card).Color.ToString().ToLower(), "");
            if (card is ChangeColorCard)
                json = new JSONCard("change_color", "", "");
            if (card is ChangeDirectionCard)
                json = new JSONCard("change_direction", ((ColorCard)card).Color.ToString().ToLower(), "");
            if (card is TakiCard)
                json = new JSONCard("taki", ((ColorCard)card).Color.ToString().ToLower(),"");
            if (card is SuperTakiCard)
                json = new JSONCard("super_taki", "","");

            return json;

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
        public Tuple<int, Color> [] GetHowManyOfAnyColor()
        {
            
            int [] numarr = new int[4] { 0, 0, 0, 0 };
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    if (((ColorCard)card).Color == Color.RED)
                        numarr[0]++;
                    if (((ColorCard)card).Color == Color.BLUE)
                        numarr[1]++;
                    if (((ColorCard)card).Color == Color.GREEN)
                        numarr[2]++;
                    if (((ColorCard)card).Color == Color.YELLOW)
                        numarr[3]++;
                }

            }
            Tuple<int, Color>[] arr = new Tuple<int, Color>[4] { Tuple.Create(numarr[0], Color.RED), Tuple.Create(numarr[1], Color.BLUE)
                , Tuple.Create(numarr[2], Color.GREEN) , Tuple.Create(numarr[3], Color.YELLOW) };
            return arr;
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

        public List<string> GetAllCardsOfColor(Color color)
        {
            List<string> lst = new List<string>();
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    if (((ColorCard)card).Color == color)
                    {
                        if (card is NumberCard)
                            lst.Add("number "+((NumberCard)card).Number.ToString());
                        if (card is StopCard)
                            lst.Add("stop");
                        if (card is PlusCard)
                            lst.Add("plus");
                        if (card is TwoPlusCard)
                            lst.Add("plus_2");
                        if (card is TakiCard)
                            lst.Add("taki");
                        if (card is ChangeDirectionCard)
                            lst.Add("change_direction");
                    }
                }
            }
            return lst;
        }

        public bool DoesHaveColor(Card card2)
        {
            Color color = new Color();
            if (card2 is ColorCard)
                color = ((ColorCard)card2).Color;
            else
                return false;
            foreach (Card card in hand)
            {
                if (card is ColorCard)
                {
                    if (((ColorCard)card).Color == color )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DoesHaveTypeOrNumber(Card card2)
        {
            foreach (Card card in hand)
            {
                if (card is TwoPlusCard && card2 is TwoPlusCard)
                {
                    return true;
                }
                if (card is PlusCard && card2 is PlusCard)
                {
                    return true;
                }
                if (card is StopCard && card2 is StopCard)
                {
                    return true;
                }
                if (card is TakiCard && card2 is TakiCard)
                {
                    return true;
                }
                if (card is ChangeDirectionCard && card2 is ChangeDirectionCard)
                {
                    return true;
                }
                if (card is NumberCard && card2 is NumberCard)
                {
                    if (((NumberCard)card).Number == ((NumberCard)card2).Number)
                        return true;
                }
            }
            return false;
        }

        public int HowManyIregularCardsOfAColor(Color color)
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

        public int HowManyTwoPlus()
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is TwoPlusCard)
                {
                    counter++;
                }
                
            }
            return counter;
        }
        public int HowManyPlus()
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is PlusCard)
                {
                    counter++;
                }

            }
            return counter;
        }
        public int HowManySuperTaki()
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is SuperTakiCard)
                {
                    counter++;
                }

            }
            return counter;
        }
        public int HowmanyTaki()
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is TakiCard)
                {
                    counter++;
                }

            }
            return counter;
        }
        public int HowManyChangeColor()
        {
            int counter = 0;
            foreach (Card card in hand)
            {
                if (card is ChangeColorCard)
                {
                    counter++;
                }

            }
            return counter;
        }
    }
}
