using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class Bot
    {
        private ActivePlayer player;
        private Game game;
  
        public Bot(Game game)
        {
            this.game = game;
            this.player = game.GetActivePlayer();
        }
        
        public void DoTurn(Client client, GameWindow gameWindow)
        {
            if(game.usedCards.Count == 0)
            {
                DoFirstTurn(client, gameWindow);
                return;
            }
            List<Card> cards = new List<Card>();
            Tuple<int, List<Color>> commonColors = player.GetCommonColor();
            if (commonColors.Item2.Contains(game.usedCards.Last().Color))
            {
                foreach (Card card in player.GetAllCardsOfColor(game.usedCards.Last().Color))
                {
                    if (cards is NumberCard)
                    {
                        cards.Add(card);
                        break;
                    }
                }
                player.PlayCard(cards, client);
                foreach(Card card in cards)
                {
                    gameWindow.AnimateUseCard(player, card);
                }
            }
            else
            {
                List<Card> drawnCards = player.DrawCard(client);
                foreach (Card card in drawnCards)
                {
                    gameWindow.AnimateUseCard(player, card);
                }
            }
        }

        private void DoFirstTurn(Client client, GameWindow gameWindow)
        {
            List<Card> cards = new List<Card>();
            Tuple<int, List<Color>> commonColors = player.GetCommonColor();

            foreach (Card card in player.GetAllCardsOfColor(commonColors.Item2[0]))
            {
                if (cards is NumberCard)
                {
                    cards.Add(card);
                    break;
                }
            }
            player.PlayCard(cards, client);
            foreach (Card card in cards)
            {
                gameWindow.AnimateUseCard(player, card);
            }
        }
    }
}
