using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class ProgressionNode
    {
        public List<ProgressionNode> ParentNodes { get; set; }
        public List<ProgressionNode> ChildrenNodes { get; set; }

        public Item KeyItemRequired { get; set; }
        public Level OwnerLevel { get; set; }
        public List<ItemLot> ItemLots { get; set; }
        public List<NPC> NPCs { get; set; }
        public NPCShopkeeper ShopKeeper { get; set; }
    }
}
