using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class JSONCard
    {
        public string type { get; set; }
        public string color { get; set; }
        public dynamic value { get; set; }
        
        public JSONCard(string type, string color, dynamic value)
        {
            this.type = type;
            this.color = color;
            this.value = value;
        }
    }
}
