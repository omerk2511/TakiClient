using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki.Cards
{
    class ChangeDirectionCard : ColorCard
    {
        public ChangeDirectionCard(Color color) : base(color) { }

        public override string Serialize()
        {
            JSONCard card = new JSONCard("change_direction", Color.ToString().ToLower(), "");
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return ("swd" + base.GetColorName());
        }
    }
}
