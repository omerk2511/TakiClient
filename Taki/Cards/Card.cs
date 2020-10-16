using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    abstract class Card
    {
        public abstract string Serialize();
        public abstract string GetResourceName();
    }
}
