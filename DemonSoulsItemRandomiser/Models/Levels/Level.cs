using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class Level
    {
        public WorldID World { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public List<ItemLot> ItemLots { get; set; }
        public List<NPCShopkeeper> ShopKeepers { get; set; }

        public Level(WorldID world, int levelID, string levelName)
        {
            World = world;
            LevelID = levelID;
            LevelName = levelName;
            ItemLots = new List<ItemLot>();
            ShopKeepers = new List<NPCShopkeeper>();
        }
    }
}
