using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EternalCalculator
{

    public enum Set { EmptyThrone, OmensOfThePast, DuskRoad }
    public enum Rarity { Common, Uncommon, Rare, Promo, Legendary }

    public class CardCollection : ICollection<Card> 
    {
        public int ShiftStoneTotal { get; private set; }
        /// <summary>
        /// Counts the total number of cards in the collection. Including duplicates.
        /// </summary>
        /// <returns>The total number of all cards in the collection.</returns>
        public int Count => CardQuantities.Select(cq => cq.Value).Aggregate(0, (count, quantity) => count + quantity);
        public bool IsReadOnly => false;
        private Dictionary<Card, int> CardQuantities;
        // TODO: CardCollection should hold and manage the master collection of cards.

        public CardCollection(int shiftStoneTotal = 0)
        {
            ShiftStoneTotal = shiftStoneTotal;
            CardQuantities = new Dictionary<Card, int>();
        }

        public CardCollection Clone()
        {
            CardCollection clone = new CardCollection(ShiftStoneTotal);
            foreach (var kvp in CardQuantities)
            {
                clone.CardQuantities[kvp.Key] = kvp.Value;
            }
            return clone;
        }

        public int DestroyExcessCards(bool lazy = true) // TODO: get rid of lazy bool input.
        {
            // TODO: Implement card destruction strageties.
            int total = 0;
            foreach (var card in CardQuantities.Keys)
            {
                // Use a lazy stragegy to destroy cards.
                // corresponds to using delete duplicates button in the menu UI.
                var value = card.GetShiftStoneValue();
                var qtyToDestroy = Math.Max(0, CardQuantities[card] - 4);
                total += value * qtyToDestroy;
                CardQuantities[card] -= qtyToDestroy;
            }
            ShiftStoneTotal += total;
            return total;
        }

        public int CostToCraftRemaingCards(Set? set = null)
        {
            // TODO: Implement crafting stratagy?
            var total = 0;
            var cardsInSet = CardQuantities.Keys as IEnumerable<Card>;
            if (set != null) {
                cardsInSet = cardsInSet.Where(c => c.Set == set);
            }
            foreach (var card in cardsInSet)
            {
                var twin = card.GetTwin();
                var qtyOwned = CardQuantities[card] + CardQuantities[twin];
                var qtyToCraft = Math.Max(0, 4 - qtyOwned);
                var costToCraft = Math.Min(card.GetShiftStoneCost(), card.GetShiftStoneCost());
                total += qtyToCraft * costToCraft;
            }
            return total;
        }

        public bool CanCraftRemainingCards(Set? set = null)
        {
            return CostToCraftRemaingCards(set) < ShiftStoneTotal;
        }

        private void CraftRemaingCards(Set? set = null)
        {
            // TODO: Implement crafting stratagy?
            int total = 0;
            var cardsInSet = CardQuantities.Keys as IEnumerable<Card>;
            if (set != null) {
                cardsInSet = cardsInSet.Where(c => c.Set == set);
            }
            foreach (var card in cardsInSet)
            {
                var twin = card.GetTwin();
                var qtyOwned = CardQuantities[card] + CardQuantities[twin];
                var qtyToCraft = Math.Max(0, 4 - qtyOwned);
                var cheaperCard = card.IsPremium ? twin : card;
                total += qtyToCraft * cheaperCard.GetShiftStoneCost();
                CardQuantities[cheaperCard] += qtyToCraft;
            }
            ShiftStoneTotal -= total;
        }

        public bool CraftRemainingCardsIfPossible(Set? set = null)
        {
            if (CanCraftRemainingCards(set))
            {
                CraftRemaingCards(set);
                return true;
            }
            return false;
        }

        public void AddPack(Pack pack)
        {
            ShiftStoneTotal += Pack.SHIFT_STONE_PER_PACK;
            foreach (var card in pack.cards)
            {
                Add(card);
            }
        }

        public void Add(Card card)
        {
            // TODO: Add check to ensure card is in the master card list.
            if (!CardQuantities.ContainsKey(card))
            {
                CardQuantities[card] = 0;
            }
            CardQuantities[card] += 1;
        }

        public bool Remove(Card card)
        {
            if (!CardQuantities.ContainsKey(card))
            {
                return false;
            }
            CardQuantities[card] -= 1;

            // Clamp quantity to zero
            CardQuantities[card] = Math.Max(CardQuantities[card], 0);
            return true;
        }

        public void Clear()
        {
            CardQuantities.Values.ToList().ForEach(cq => cq = 0);
        }

        public void ResetShiftStoneTotal()
        {
            ShiftStoneTotal = 0;
        }

        public void Reset()
        {
            Clear();
            ResetShiftStoneTotal();
        }

        public bool Contains(Card item)
        {
            return CardQuantities.ContainsKey(item) && CardQuantities[item]> 0;
        }

        /// <summary>
        /// Copies one instance of each card in the collection to the input array.
        /// </summary>
        /// <param name="array">The array the cards in the collection will be copied to.</param>
        /// <param name="index">The first index of the array to receive a copied value.</param>
        public void CopyTo(Card[] array, int index = 0)
        {
            CardQuantities.Keys.Where(c => CardQuantities[c] > 0).ToList().CopyTo(array, index);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}
