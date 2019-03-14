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
            switch (Rarity)
            {
                case Rarity.Common:
                    return IsPremium ? 25 : 1;
                case Rarity.Uncommon:
                    return IsPremium ? 50 : 10;
                case Rarity.Rare:
                    return IsPremium ? 800 : 200;
                case Rarity.Legendary:
                    return IsPremium ? 3200 : 800;
                case Rarity.Promo:
                    return IsPremium ? 400 : 100;
            }
            return 0;
        }

        public int GetShiftStoneCost()
        {
            switch (Rarity)
            {
                case Rarity.Common:
                    return IsPremium ? 800 : 50;
                case Rarity.Uncommon:
                    return IsPremium ? 1600 : 100;
                case Rarity.Rare:
                    return IsPremium ? 3200 : 800;
                case Rarity.Legendary:
                    return IsPremium ? 9600 : 3200;
                case Rarity.Promo:
                    return IsPremium ? 2400 : 600;
            }
            return 0;
        }
    }
}
