using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Cards;

namespace Taki
{
    class NonActivePlayer : Player
    {
        private int _cardCount;
        public int cardCount
        {
            get { return _cardCount; }
            set { _cardCount = value; }
        }

        public NonActivePlayer(int cardCount)
        {
            this._cardCount = cardCount;
        }

        public void RemoveCard()
        {
            this._cardCount--;
        }

        public void AddCard()
        {
            this._cardCount++;
        }
        
        public override int GetCardAmount()
        {
            return this._cardCount;
        }

        public override List<string> GetCardResources()
        {
            List<string> lst = new List<string>(this.cardCount);
            for(int i = 0; i < this.cardCount; i++)
            {
                lst.Add("back");
            }
            return lst;
        }

    }
}
