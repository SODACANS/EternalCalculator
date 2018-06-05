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

        public int GetShiftStoneValue()
        {
            if (IsPremium)
            {
                switch (Rarity)
                {
                    case Rarity.Common:
                        return 25;
                    case Rarity.Uncommon:
                        return 50;
                    case Rarity.Rare:
                        return 800;
                    case Rarity.Legendary:
                        return 3200;
                    case Rarity.Promo:
                        return 400;
                }
            }
            else
            {
                switch (Rarity)
                {
                    case Rarity.Common:
                        return 1;
                    case Rarity.Uncommon:
                        return 10;
                    case Rarity.Rare:
                        return 200;
                    case Rarity.Legendary:
                        return 800;
                    case Rarity.Promo:
                        return 100;
                }
            }
            return 0;
        }

        public int GetShiftStoneCost()
        {
            if (IsPremium)
            {
                switch (Rarity)
                {
                    case Rarity.Common:
                        return 800;
                    case Rarity.Uncommon:
                        return 1600;
                    case Rarity.Rare:
                        return 3200;
                    case Rarity.Legendary:
                        return 9600;
                    case Rarity.Promo:
                        return 2400;
                }
            }
            else
            {
                switch (Rarity)
                {
                    case Rarity.Common:
                        return 50;
                    case Rarity.Uncommon:
                        return 100;
                    case Rarity.Rare:
                        return 800;
                    case Rarity.Legendary:
                        return 3200;
                    case Rarity.Promo:
                        return 600;
                }
            }
            return 0;
        }
    }
}
