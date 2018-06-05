using System;
using System.Collections.Generic;
using System.Text;

namespace EternalCalculator
{
    public class Pack
    {
        public Set Set;
        public List<Card> cards;

        public Pack(Set set)
        {
            Set = set;
            cards = new List<Card>();
        }
    }
}
