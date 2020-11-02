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
        public override JSONCard Serialize()
        {
            return(new JSONCard("change_color", "", ""));
        }

        public override string GetResourceName()
        {
            return ("s_swc");
        }
    }
}
