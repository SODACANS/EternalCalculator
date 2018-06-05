using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalCalculator
{
    public class CardCollection
    {
        public Dictionary<Set, SetList> Sets;

        public CardCollection()
        {
            Set[] sets = (Set[])Enum.GetValues(typeof(Set));
            Sets = sets.ToDictionary(s => s, s => new SetList(s));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n");
            string setName;
            string rarityName;
            SetList setList;
            RarityGroup rarityGroup;
            Card card, premiumCard;
            foreach(Set set in Sets.Keys)
            {
                setName = Enum.GetName(typeof(Set), set);
                setList = Sets[set];
                sb.Append("\t" + setName + "\n");
                foreach(Rarity rarity in setList.RarityGroups.Keys)
                {
                    rarityName = Enum.GetName(typeof(Rarity), rarity);
                    rarityGroup = setList.RarityGroups[rarity];
                    sb.Append("\t\t" + rarityName + "\n");
                    foreach(string cardName in rarityGroup.Cards.Keys)
                    {
                        card = rarityGroup.Cards[cardName];
                        premiumCard = rarityGroup.PremiumCards[cardName];
                        sb.Append("\t\t\t" + card.Name + " " + card.Quantity + " " + premiumCard.Quantity + "\n");
                    }
                }
            }
            
            return sb.ToString();
        }

        public CardCollection Clone()
        {
            CardCollection clone = new CardCollection();
            foreach (SetList setList in Sets.Values)
            {
                foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
                {
                    foreach (Card card in rarityGroup.Cards.Values)
                    {
                        clone.Sets[setList.Set].RarityGroups[rarityGroup.Rarity].Cards[card.Name] = card.Clone();
                    }
                    foreach (Card card in rarityGroup.PremiumCards.Values)
                    {
                        clone.Sets[setList.Set].RarityGroups[rarityGroup.Rarity].PremiumCards[card.Name] = card.Clone();
                    }
                }
            }
            return clone;
        }

        public void AddPack(Pack pack)
        {
            foreach(Card card in pack.cards)
            {
                if (card.IsPremium)
                {
                    Sets[pack.Set].RarityGroups[card.Rarity].PremiumCards[card.Name].Quantity++;
                }
                else
                {
                    Sets[pack.Set].RarityGroups[card.Rarity].Cards[card.Name].Quantity++;
                }
            }
        }
    }
}
