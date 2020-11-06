using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taki.Cards
{
    class SuperTakiCard : Card, ISpecialCard
    {
        public override JSONCard Serialize()
        {
            return new JSONCard("super_taki", "", "");
        }

        public override string GetResourceName()
        {
            return ("s_stk");
        }

        JSONCard ISpecialCard.SerializeColor(Color color)
        {
            return new JSONCard("super_taki", color.ToString().ToLower(), "");
        }

    }
}
