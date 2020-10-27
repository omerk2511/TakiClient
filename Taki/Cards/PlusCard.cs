using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki.Cards
{
    class PlusCard : ColorCard
    {
        public PlusCard(Color color) : base(color) { }

        public override JSONCard Serialize()
        {
            return new JSONCard("plus", Color.ToString().ToLower(), "");
        }

        public override string GetResourceName()
        {
            return ("pls" + base.GetColorName());
        }
    }
}
