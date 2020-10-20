using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki.Cards
{
    class ChangeColorCard: Card
    {
        public override string Serialize()
        {
            JSONCard card = new JSONCard("change_color", "", "");
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return ("s_swc");
        }
    }
}
