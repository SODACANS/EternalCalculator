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

        public int GetShiftStoneValueOfExcessCards(bool lazy = true)
        {
            int total = 0;
            foreach (SetList setList in Sets.Values)
            {
                foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
                {
                    if (lazy)
                    {
                        int value = 0;
                        int qty = 0;
                        foreach (Card card in rarityGroup.Cards.Values)
                        {
                            value = card.GetShiftStoneValue();
                            qty = Math.Max(0, card.Quantity - 4);
                            total += value * qty;
                        }
                        foreach (Card card in rarityGroup.PremiumCards.Values)
                        {
                            value = card.GetShiftStoneValue();
                            qty = Math.Max(0, card.Quantity - 4);
                            total += value * qty;
                        }
                    }
                    else
                    {
                        Card premiumCard;
                        int totalQty, qtyToDestroy, qtyNormal, qtyPremium;
                        foreach (Card card in rarityGroup.Cards.Values)
                        {
                            premiumCard = rarityGroup.PremiumCards[card.Name];
                            totalQty = card.Quantity + premiumCard.Quantity;
                            qtyToDestroy = Math.Max(0, totalQty - 4);
                            if (qtyToDestroy > 0)
                            {
                                qtyNormal = Math.Min(card.Quantity, qtyToDestroy);
                                qtyPremium = qtyToDestroy - qtyNormal;
                                total += card.GetShiftStoneValue() * qtyNormal + premiumCard.GetShiftStoneValue() * qtyPremium;
                            }
                        }
                    }
                }
            }
            return total;
        }
    }
}
