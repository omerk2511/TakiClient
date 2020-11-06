using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    class Bot
    {
        private ActivePlayer player;
        private Game game;
        private Client client;
        private GameWindow gameWindow;
  
        public Bot(Game game, Client client, GameWindow gameWindow)
        {
            this.game = game;
            this.client = client;
            this.gameWindow = gameWindow;
            this.player = game.GetActivePlayer();
        }
        

        private void HandlePlusTwo(List<Card> cardsToAdd)
        {
            List<Card> twoPlusCards = player.GetCardWithTypeOrNumber(new TwoPlusCard(Color.UNDEFINED));
            if (twoPlusCards.Count != 0)
            {
                PlayCards(new List<Card> { twoPlusCards[0] }, Color.UNDEFINED);
            }
            else
            {
                DrawCards();
            }
        }
        
        private void HandleTakiSuperTaki(List<Card> cardsToAdd, Card lastCard, Color superTakiColor)
        {
            List<Card> colorCards;
            if (superTakiColor == Color.UNDEFINED)
            {
                colorCards = player.GetAllCardsOfColor(superTakiColor);
            }
            else
            {
                colorCards = player.GetAllCardsOfColor(((ColorCard)lastCard).Color);
            }
            if (colorCards.Count != 0)
            {
                cardsToAdd.AddRange(colorCards.Where(card => card != lastCard && card is TakiCard));
                cardsToAdd.AddRange(colorCards.Where(card => card != lastCard && card is NumberCard));
                cardsToAdd.AddRange(colorCards.Where(card => card != lastCard && !(card is NumberCard) && !(card is TwoPlusCard)));
                cardsToAdd.AddRange(colorCards.Where(card => card is TwoPlusCard));
                PlayCards(cardsToAdd, superTakiColor);
            }
        }

        private void HandlePlus(List<Card> cardsToAdd)
        {
            List<Card> colorCards = player.GetAllCardsOfColor(((ColorCard)cardsToAdd.Last()).Color);
            foreach (Card card in colorCards)
            {
                if (card == cardsToAdd.Last())
                {
                    colorCards.Remove(card);
                    break;
                }
            }
            if(colorCards.Count != 0)
            { 
                cardsToAdd.Add(colorCards[0]);
            }
            else
            {
                List<Card> valueCards = player.GetCardWithTypeOrNumber(cardsToAdd.Last());
                foreach (Card card in valueCards)
                {
                    if (card == cardsToAdd.Last())
                    {
                        valueCards.Remove(card);
                        break;
                    }
                }
                if (valueCards.Count != 0)
                {
                    cardsToAdd.Add(valueCards[0]);
                    HandlePlus(cardsToAdd);
                }
            }
        }

        public void DoTurn(List<Card> cardsToAdd)
        {
            if (game.usedCards.Count == 0)
            {
                DoFirstTurn();
                return;
            }
            Card lastUsedCard = game.usedCards.Last();
            
            if(lastUsedCard is TwoPlusCard)
            {
                HandlePlusTwo(cardsToAdd);
                return;
            }

            if(lastUsedCard is TakiCard)
            {
                HandleTakiSuperTaki(cardsToAdd, lastUsedCard, Color.UNDEFINED);
            }

            List<Card> takiCards = player.GetCardWithTypeOrNumber(new TakiCard(Color.UNDEFINED));
            int takiIndex = takiCards.FindIndex(card => ((ColorCard)card).Color == ((ColorCard)lastUsedCard).Color);
            if(takiIndex != -1)
            {
                cardsToAdd.Add(takiCards[takiIndex]);
                HandleTakiSuperTaki(cardsToAdd, takiCards[takiIndex], Color.UNDEFINED);
                return;
            }

            List<Card> validCardsColor = player.GetAllCardsOfColor(game.usedCards.Last().Color);
            foreach (Card card in validCardsColor)
            {
                if (card is NumberCard || card is StopCard || card is ChangeDirectionCard)
                {
                    cardsToAdd.Add(card);
                    break;
                }
                else if(card is PlusCard)
                {
                    cardsToAdd.Add(card);
                    HandlePlus(cardsToAdd);
                }
            }

            if(cardsToAdd.Count == 0)
            {
                foreach (Card card in player.GetCardWithTypeOrNumber(game.usedCards.Last()))
                {
                    if (card is NumberCard)
                    {
                        cardsToAdd.Add(card);
                        break;
                    }
                }   
            }


            if(cardsToAdd.Count == 0)
            {
                // Try to use SuperTaki or ChangeColor
                SuperTakiCard superTaki = player.GetCardOfType<SuperTakiCard>();
                if (superTaki != null)
                {
                    cardsToAdd.Add(superTaki);
                    HandleTakiSuperTaki(cardsToAdd, superTaki, player.GetCommonColor().Item2[0]);
                }
                else
                {
                    ChangeColorCard chageColorCard = player.GetCardOfType<ChangeColorCard>();
                    if (chageColorCard != null)
                    {
                        cardsToAdd.Add(chageColorCard);
                        PlayCards(cardsToAdd, player.GetCommonColor().Item2[0]);
                    }
                }

            }

            if (cardsToAdd.Count == 0)
            {
                DrawCards();
            }
            else
            {
                PlayCards(cardsToAdd, Color.UNDEFINED);
            }
        }

        private void DoFirstTurn()
        { 
            List<Card> cards = new List<Card>();
            Tuple<int, List<Color>> commonColors = player.GetCommonColor();

            foreach (Card card in player.GetAllCardsOfColor(commonColors.Item2[0]))
            {
                if (card is NumberCard)
                {
                    cards.Add(card);
                    break;
                }
            }
            PlayCards(cards, Color.UNDEFINED);

        }

        private void DrawCards()
        {
            List<Card> drawnCards = player.DrawCard(client);
            foreach (Card card in drawnCards)
            {
                gameWindow.AnimateDrawCard(player, card);
            }
        }
        
        private void PlayCards(List<Card> cards, Color specialColor)
        {
            player.PlayCard(cards, client, specialColor);
            foreach (Card card in cards)
            {
                gameWindow.AnimateUseCard(player, card);
            }
        }
    }
}
