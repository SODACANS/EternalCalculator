using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalCalculator
{
    public class PackFactory
    {
        public const double LEGENDARY_CHANCE = 0.11;
        public const double PREMIUM_CHANCE = 0.01;
        public const int NUM_COMMONS = 8;
        public const int NUM_UNCOMMONS = 3;
        public const int NUM_RARE_LEGENDARY = 1;

        private CardCollection MasterCardCollection;

        public PackFactory(CardCollection masterCardCollection)
        {
            MasterCardCollection = masterCardCollection;
        }

        public Pack FillPack(Set set)
        {
            Pack pack = new Pack(set);
            Random random = new Random();

            // stick in 8 commons
            SetList setList = MasterCardCollection.Sets[set];
            Rarity rarity = Rarity.Common;
            RarityGroup rarityGroup = setList.RarityGroups[rarity];
            Card[] cardList = rarityGroup.Cards.Select(kvp => kvp.Value).ToArray();
            int cardPoolSize = cardList.Length;
            Card newCard;
            int index;
            for (int i = 0; i < NUM_COMMONS; i++)
            {
                index = random.Next(cardPoolSize);
                newCard = cardList[index].Clone();
                pack.cards.Add(newCard);
            }

            // stick in 3 uncommons
            rarity = Rarity.Uncommon;
            rarityGroup = setList.RarityGroups[rarity];
            cardList = rarityGroup.Cards.Select(kvp => kvp.Value).ToArray();
            cardPoolSize = cardList.Length;
            for (int i = 0; i < NUM_UNCOMMONS; i++)
            {
                index = random.Next(cardPoolSize);
                newCard = cardList[index].Clone();
                pack.cards.Add(newCard);
            }

            // stick in 1 rare/legendary
            if (random.NextDouble() < LEGENDARY_CHANCE)
            {
                // The pack contains a legendary
                rarity = Rarity.Legendary;
                rarityGroup = setList.RarityGroups[rarity];
                cardList = rarityGroup.Cards.Select(kvp => kvp.Value).ToArray();
                cardPoolSize = cardList.Length;
                index = random.Next(cardPoolSize);
                newCard = cardList[index].Clone();
                pack.cards.Add(newCard);
            }
            else
            {
                // The pack contains a rare
                rarity = Rarity.Rare;
                rarityGroup = setList.RarityGroups[rarity];
                cardList = rarityGroup.Cards.Select(kvp => kvp.Value).ToArray();
                cardPoolSize = cardList.Length;
                index = random.Next(cardPoolSize);
                newCard = cardList[index].Clone();
                pack.cards.Add(newCard);
            }

            // Now loop through each card and role to see if it is a premium card
            foreach(Card card in pack.cards)
            {
                if (random.NextDouble() < PREMIUM_CHANCE)
                {
                    card.IsPremium = true;
                }
            }

            return pack;
        }
    }
}
