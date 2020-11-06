using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki.Cards
{
    interface ISpecialCard
    {
        JSONCard SerializeColor(Color color);
    }
}
