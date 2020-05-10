using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class NPC
    {
        string Name { get; set; }
        ItemLot ItemLot { get; set; }

        public NPC(ItemLot itemLot)
        {
            ItemLot = itemLot;
        }

        public override string ToString()
        {
            return Name + " - " + ItemLot.Description;
        }
    }
}
