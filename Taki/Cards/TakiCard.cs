using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki.Cards
{
    class TakiCard : ColorCard
    {
        public TakiCard(Color color) : base(color) { }

        public override JSONCard Serialize()
        { 
            return new JSONCard("taki", Color.ToString().ToLower(), "");
        }

        public override string GetResourceName()
        {
            return ("tki" + base.GetColorName());
        }
    }
}
