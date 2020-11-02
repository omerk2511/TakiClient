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

        public override JSONCard Serialize()
        {
            return new JSONCard("change_direction", Color.ToString().ToLower(), "");
        }

        public override string GetResourceName()
        {
            return ("swd" + base.GetColorName());
        }
    }
}
