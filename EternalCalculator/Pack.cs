using System.Collections.Generic;

namespace EternalCalculator
{
    public class Pack
    {
        public const int SHIFT_STONE_PER_PACK = 100;
        // TODO: Find a better home for this constant. Maybe a ResultsAnalyzer class?
        public const decimal GEMS_PER_DOLLAR = 116.6861143523921m;
        public const decimal GEMS_PER_PACK = 90.625m;
        public Set Set { get; }
        public ICollection<Card> cards;

        public Pack(Set set)
        {
            Set = set;
            cards = new List<Card>();
        }
    }
}
