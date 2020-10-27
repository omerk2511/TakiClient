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

        public override JSONCard Serialize()
        {
            return new JSONCard("plus_2", Color.ToString().ToLower(), "");
        }

        public override string GetResourceName()
        {
            return ("_2" + base.GetColorName());
        }

    }
}
