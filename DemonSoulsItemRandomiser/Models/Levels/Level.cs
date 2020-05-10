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
        public int WorldID { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public List<ItemLot> ItemLots { get; set; }
    }
}
