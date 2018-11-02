using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EternalCalculator
{
    public enum Rarity { Common, Uncommon, Rare, Promo, Legendary }


    public class RarityGroup
    {
        public const string basePathToMasterCardLists = @"D:\Developement\Projects\EternalCalculator\EternalCalculator\MasterCardLists";

        public Set Set;
        public Rarity Rarity;
        /// <summary>
        /// Maps a card name to a Card object. This dictionary represents the non-premium cards.
        /// </summary>
        public Dictionary<string, Card> Cards = new Dictionary<string, Card>();
        public Dictionary<string, Card> PremiumCards = new Dictionary<string, Card>();

        public RarityGroup(Set set, Rarity rarity)
        {
            Set = set;
            Rarity = rarity;
        }

        public void Initialize()
        {
            string setName = Set.ToString();
            string rarityName = Rarity.ToString();
            string path = Path.Combine(basePathToMasterCardLists, setName, rarityName + ".txt");
            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach(string line in lines)
                {
                    string cardName = ParseCardNameFromLine(line);
                    Card card = new Card(cardName, Set, Rarity);
                    Cards[cardName] = card;
                    Card premiumCard = card.Clone();
                    premiumCard.IsPremium = true;
                    PremiumCards[cardName] = premiumCard;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read cards from file {0}", path);
                Console.WriteLine("Reason: {0}", ex.Message);
            }
        }

        private static string ParseCardNameFromLine(string line)
        {
            int endIndex = line.IndexOf('(') - 1;
            int startIndex = 2;
            int length = endIndex - startIndex;
            return line.Substring(startIndex, length);
        }
    }
}
