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

        public override JSONCard Serialize()
        {
            return new JSONCard("stop", Color.ToString().ToLower(), "");
        }

        public override string GetResourceName()
        {
            return ("stp" + base.GetColorName() );
        }
    }
}
