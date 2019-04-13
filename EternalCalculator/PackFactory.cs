using System;
using System.Linq;

namespace EternalCalculator
{
    public class PackFactory
    {
        public const double LEGENDARY_CHANCE = 0.11;
        public const double PREMIUM_CHANCE = 0.01;
        public const int NUM_COMMONS = 8;
        public const int NUM_UNCOMMONS = 3;
        public const int NUM_RARE_LEGENDARY = 1;

        public Pack FillPack(Set set)
        {
            Pack pack = new Pack(set);
            Random random = new Random();
            var cardsInSet = CardCollection.MasterCardCollection.Where(c => c.Set == set && !c.IsPremium);

            // stick in 8 commons
            var cardList = cardsInSet.Where(c => c.Rarity == Rarity.Common).ToList();
            int cardPoolSize = cardList.Count;
            for (int i = 0; i < NUM_COMMONS; i++)
            {
                var index = random.Next(cardPoolSize);
                pack.cards.Add(cardList[index]);
            }

            // stick in 3 uncommons
            cardList = cardsInSet.Where(c => c.Rarity == Rarity.Uncommon).ToList();
            cardPoolSize = cardList.Count;
            for (int i = 0; i < NUM_UNCOMMONS; i++)
            {
                var index = random.Next(cardPoolSize);
                pack.cards.Add(cardList[index]);
            }

            // stick in 1 rare/legendary
            if (random.NextDouble() < LEGENDARY_CHANCE)
            {
                // The pack contains a legendary
                cardList = cardsInSet.Where(c => c.Rarity == Rarity.Legendary).ToList();
                cardPoolSize = cardList.Count;
                var index = random.Next(cardPoolSize);
                pack.cards.Add(cardList[index]);
            }
            else
            {
                // The pack contains a rare
                cardList = cardsInSet.Where(c => c.Rarity == Rarity.Rare).ToList();
                cardPoolSize = cardList.Count;
                var index = random.Next(cardPoolSize);
                pack.cards.Add(cardList[index]);
            }

            // Now roll to see if each card is a premium card.
            pack.cards = pack.cards.Select(c => random.NextDouble() < PREMIUM_CHANCE ? c.GetTwin() : c).ToList();

            return pack;
        }
    }
}
