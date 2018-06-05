using System;
using System.Collections.Generic;
using System.Text;

namespace EternalCalculator
{
    public class Card
    {
        public string Name;
        public Set Set;
        public Rarity Rarity;
        public bool IsPremium = false;
        public int Quantity = 0;

        public Card(string name, Set set, Rarity rarity, bool isPremium = false, int quantity = 0)
        {
            Name = name;
            Set = set;
            Rarity = rarity;
            IsPremium = isPremium;
            Quantity = quantity;
        }

        public Card Clone()
        {
            return new Card(Name, Set, Rarity, IsPremium, Quantity);
        }
    }
}
