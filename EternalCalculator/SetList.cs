using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalCalculator
{
    public enum Set { EmptyThrone, OmensOfThePast, DuskRoad }

    public class SetList
    {
        public Set Set;
        public Dictionary<Rarity, RarityGroup> RarityGroups;

        public SetList(Set set)
        {
            Set = set;
            Rarity[] rarities = (Rarity[])Enum.GetValues(typeof(Rarity));
            RarityGroups = rarities.ToDictionary(r => r, r => new RarityGroup(set, r));
        }
    }
}
