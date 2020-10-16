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

        public override string Serialize()
        {
            JSONCard card = new JSONCard("taki", Color.ToString().ToLower(), "");
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return ("tki" + base.GetColorName());
        }
    }
}
