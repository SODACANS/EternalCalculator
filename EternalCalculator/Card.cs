using System;

namespace EternalCalculator
{
    public struct Card: IEquatable<Card>
    {
        public string Name { get; }
        public Set Set { get; }
        public Rarity Rarity { get; }
        public bool IsPremium { get; }

        public Card(string name, Set set, Rarity rarity, bool isPremium = false )
        {
            Name = name;
            Set = set;
            Rarity = rarity;
            IsPremium = isPremium;
        }

        /// <summary>
        /// Gets the premium version of this card if it is standard, and vice-a-versa.
        /// </summary>
        public Card GetTwin()
        {
            return new Card(Name, Set, Rarity, !IsPremium);
        }

        // This is a good candidate for unit tests
        // see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
        // for requirements.
        public override bool Equals(object obj)
        {
            // The operations (obj is Card card) asserts that obj is of type
            // `Card`. If it is of the correct type it is then cast and the result is put in `card`
            return obj is Card card && Equals(card);
        }

        public  bool Equals(Card card)
        {
            return Name == card.Name
                && Set == card.Set
                && Rarity == card.Rarity
                && IsPremium == card.IsPremium;
        }

        // Best attempt at providing a hash function.
        // Based on advise here: https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netframework-4.7.2
        // and here: https://stackoverflow.com/questions/113511/best-implementation-for-hashcode-method-for-a-collection
        // would be good to test empirically.
        public override int GetHashCode()
        {
            var result = Name.GetHashCode();
            const int factor = 31;
            result = factor * result + (int)Set;
            result = factor * result + (int)Rarity;
            result = factor * result + (IsPremium ? 1 : 0);
            return result;
        }

        public static bool operator ==(Card lhs, Card rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Card lhs, Card rhs)
        {
            return !lhs.Equals(rhs);
        }

        public Card Clone()
        {
            return new Card(Name, Set, Rarity, IsPremium);
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
