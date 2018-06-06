using System;
using System.Collections.Generic;
using System.Text;

namespace EternalCalculator
{
    public class Pack
    {
        public const int SHIFT_STONE = 100;
        public const double GEMS_PER_DOLLAR = 116.6861143523921;
        public const double GEMS_PER_PACK = 90.625;
        public Set Set;
        public List<Card> cards;

        public Pack(Set set)
        {
            Set = set;
            cards = new List<Card>();
        }
    }
}
