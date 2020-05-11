using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemonSoulsItemRandomiser.Models;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class NPCShopkeeper
    {
        public string Name { get; set; }
        public List<ShopLot> Items {get; set;}
        public Level OwnerLevel { get; set; }

        public NPCShopkeeper(string name, Level ownerLevel)
        {
            Name = name;
            Items = new List<ShopLot>();
            OwnerLevel = ownerLevel;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
