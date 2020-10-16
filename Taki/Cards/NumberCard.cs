using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Taki
{
    class NumberCard : ColorCard
    {
        private static int[] VALID_NUMBERS = { 1, 3, 4, 5, 6, 7, 8, 9 };

        private int _number;
        public int Number
        {
            get { return _number; }
            set {
                if (!VALID_NUMBERS.Contains(value)) {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(value)} must be 1 or 3-9.");
                }

                _number = value;
            }
        }

        public NumberCard(int number, Color color) : base(color)
        {
            Number = number;
        }

        public override string Serialize()
        {
            JSONCard card = new JSONCard("number_card",  Color.ToString().ToLower(), Number.ToString());
            string jsonString = JsonSerializer.Serialize(card);
            return jsonString;
        }

        public override string GetResourceName()
        {
            return(Number.ToString() + base.GetColorName());
        }
    }
}
