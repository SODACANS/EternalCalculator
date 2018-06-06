using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalCalculator
{
    public class CardCollection
    {
        public int ShiftStoneTotal;
        public Dictionary<Set, SetList> Sets;

        public CardCollection()
        {
            ShiftStoneTotal = 0;
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
            ShiftStoneTotal += Pack.SHIFT_STONE;
        }

        public int DestroyExcessCards(bool lazy = true)
        {
            int total = 0;
            foreach (SetList setList in Sets.Values)
            {
                foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
                {
                    if (lazy)
                    {
                        int value = 0;
                        int qtyToDestroy = 0;
                        foreach (Card card in rarityGroup.Cards.Values)
                        {
                            value = card.GetShiftStoneValue();
                            qtyToDestroy = Math.Max(0, card.Quantity - 4);
                            total += value * qtyToDestroy;
                            card.Quantity -= qtyToDestroy;
                        }
                        foreach (Card card in rarityGroup.PremiumCards.Values)
                        {
                            value = card.GetShiftStoneValue();
                            qtyToDestroy = Math.Max(0, card.Quantity - 4);
                            total += value * qtyToDestroy;
                            card.Quantity -= qtyToDestroy;
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
                                card.Quantity -= qtyNormal;
                                premiumCard.Quantity -= qtyPremium;
                            }
                        }
                    }
                }
            }
            ShiftStoneTotal += total;
            return total;
        }

        public int CostToCraftRemaingCards(Set set)
        {
            int total = 0;
            SetList setList = Sets[set];
            foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
            {
                foreach (Card card in rarityGroup.Cards.Values)
                {
                    Card PremiumCard = rarityGroup.PremiumCards[card.Name];
                    int qtyOwned = card.Quantity + PremiumCard.Quantity;
                    int qtyToCraft = Math.Max(0, 4 - qtyOwned);
                    total += qtyToCraft * card.GetShiftStoneCost();
                }
            }
            return total;
        }

        public bool CanCraftRemainingCards(Set set)
        {
            return CostToCraftRemaingCards(set) < ShiftStoneTotal;
        }

        public void CraftRemaingCards(Set set)
        {
            int total = 0;
            SetList setList = Sets[set];
            foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
            {
                foreach (Card card in rarityGroup.Cards.Values)
                {
                    Card PremiumCard = rarityGroup.PremiumCards[card.Name];
                    int qtyOwned = card.Quantity + PremiumCard.Quantity;
                    int qtyToCraft = Math.Max(0, 4 - qtyOwned);
                    total += qtyToCraft * card.GetShiftStoneCost();
                    card.Quantity += qtyToCraft;
                }
            }
            ShiftStoneTotal -= total;
        }

        public void Reset()
        {
            foreach (SetList setList in Sets.Values)
            {
                foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
                {
                    foreach (Card card in rarityGroup.Cards.Values)
                    {
                        card.Quantity = 0;
                        rarityGroup.PremiumCards[card.Name].Quantity = 0;
                    }
                }
            }
            ShiftStoneTotal = 0;
        }
    }
}
