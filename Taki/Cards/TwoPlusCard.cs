using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki
{
    class TwoPlusCard: ColorCard
    {
        public TwoPlusCard(Color color) : base(color) { }

        public override string Serialize()
        {
            JSONCard card = new JSONCard("plus_2", Color.ToString().ToLower(), "");
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return ("2" + base.GetColorName());
        }

    }
}
