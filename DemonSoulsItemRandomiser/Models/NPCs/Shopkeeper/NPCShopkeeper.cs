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
        public List<ItemShop> Items {get; set;}
        public Level OwnerLevel { get; set; }

        public NPCShopkeeper(string name, Level ownerLevel)
        {
            Name = name;
            Items = new List<ItemShop>();
            OwnerLevel = ownerLevel;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
