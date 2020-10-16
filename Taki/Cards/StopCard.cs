using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki
{
    class StopCard: ColorCard
    {
        public StopCard(Color color) : base(color) { }

        public override string Serialize()
        {
            JSONCard card = new JSONCard("stop", Color.ToString().ToLower(), "");
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return ("stp" + base.GetColorName() );
        }
    }
}
