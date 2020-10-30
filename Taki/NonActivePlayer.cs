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

        public NonActivePlayer(string name, int cardCount) : base(name)
        {
            this._cardCount = cardCount;
        }

        public void RemoveCards(int num)
        {
            this._cardCount -= 1;
        }

        public void AddCards(int num)
        {
            this._cardCount += num;
        }
        
        public override int GetCardAmount()
        {
            return this._cardCount;
        }

        public override List<string> GetCardResources()
        {
            List<string> lst = new List<string>(this._cardCount);
            for(int i = 0; i < this._cardCount; i++)
            {
                lst.Add("back");
            }
            return lst;
        }

    }
}
