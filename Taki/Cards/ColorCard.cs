using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    enum Color
    {
        RED, BLUE, GREEN, YELLOW, UNDEFINED
    }

    abstract class ColorCard : Card
    {
        protected Color _color;
        public Color Color {
            get { return _color; }
            set { _color = value; }
        }

        public ColorCard(Color color) : base()
        {
            _color = color;
        }

        protected char GetColorName()
        {
            return _color.ToString().ToLower()[0];
        }
    }
}
