using EternalCalculator.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EternalCalculator
{
    // TODO: Need to extract intialization logic for the master collection.
    //public class RarityGroup
    //{
    //    public Set Set;
    //    public Rarity Rarity;
    //    /// <summary>
    //    /// Maps a card name to a Card object. This dictionary represents the non-premium cards.
    //    /// </summary>
    //    public Dictionary<string, Card> Cards = new Dictionary<string, Card>();
    //    public Dictionary<string, Card> PremiumCards = new Dictionary<string, Card>();

    //    public RarityGroup(Set set, Rarity rarity)
    //    {
    //        Set = set;
    //        Rarity = rarity;
    //    }

    //    public void Initialize()
    //    {
    //        // Get the card list for this group from the coresponding resource file.
    //        string resourceName = $"{Set}_{Rarity}";
    //        var cardList = Resources.ResourceManager.GetString(resourceName);
    //        var lines = cardList.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    //        // Parse the lines into our data structure.
    //        foreach(string line in lines)
    //        {
    //            string cardName = ParseCardNameFromLine(line);
    //            Card card = new Card(cardName, Set, Rarity);
    //            Cards[cardName] = card;
    //            Card premiumCard = card.Clone();
    //            premiumCard.IsPremium = true;
    //            PremiumCards[cardName] = premiumCard;
    //        }
    //    }

    //    private static string ParseCardNameFromLine(string line)
    //    {
    //        int endIndex = line.IndexOf('(') - 1;
    //        int startIndex = 2;
    //        int length = endIndex - startIndex;
    //        return line.Substring(startIndex, length);
    //    }
    //}
}
