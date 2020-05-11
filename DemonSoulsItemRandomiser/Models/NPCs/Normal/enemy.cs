using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class Enemy
    {
        public string Name { get; set; }
        public Level OwnerLevel { get; set; }
        public ItemLot ItemLot { get; set; }

        public Enemy(string name, Level ownerLevel, PARAM.Row itemLot)
        {
            Name = name;
            OwnerLevel = ownerLevel;
            ItemLot = new ItemLot(itemLot);
        }
    }
}
