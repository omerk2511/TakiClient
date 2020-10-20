using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class TakiServerException : Exception
    {
        public TakiServerException(string message) : base(message) { }
    }
}
