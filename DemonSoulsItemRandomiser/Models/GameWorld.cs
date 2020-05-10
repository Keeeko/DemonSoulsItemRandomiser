using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class GameWorld
    {
        public List<ItemLot> ItemLots;
        public List<Level> GameLevels;

        public GameWorld()
        {
            ItemLots = new List<ItemLot>();

            //Create level objects
            GameLevels = new List<Level>
            {
                new Level(WorldID.TUTORIAL, 0, "Tutorial Area"),
                new Level(WorldID.TUTORIAL, 1, "Tutorial Area"),
                new Level(WorldID.TUTORIAL, 2, "Tutorial Area"),
                new Level(WorldID.NEXUS, 0, "The Nexus"),
                new Level(WorldID.NEXUS, 1, "Endgame"),
                new Level(WorldID.BOLETARIA, 0, "Boletarian Palace"),
                new Level(WorldID.BOLETARIA, 1, "Phalanx Archstone"),
                new Level(WorldID.BOLETARIA, 2, "Tower Knight Archstone"),
                new Level(WorldID.BOLETARIA, 3, "Penetrator Archstone"),
                new Level(WorldID.STONEFANG_TUNNEL, 0, "Stonefang Tunnel"),
                new Level(WorldID.STONEFANG_TUNNEL, 1, "Armor Spider Archstone"),
                new Level(WorldID.STONEFANG_TUNNEL, 2, "Flamelurker Archstone"),
                new Level(WorldID.TOWER_OF_LATRIA, 0, "Tower of Latria"),
                new Level(WorldID.TOWER_OF_LATRIA, 1, "Fools Idol Archstone"),
                new Level(WorldID.TOWER_OF_LATRIA, 2, "Maneater Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 0, "Shrine of Storms"),
                new Level(WorldID.SHRINE_OF_STORMS, 1, "Adjudicator Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 2, "Old Hero Archstone"),
                new Level(WorldID.VALLEY_OF_DEFILEMENT, 0, "Valley of Defilement"),
                new Level(WorldID.SHRINE_OF_STORMS, 1, "Leechmonger Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 2, "Dirty Colossus Archstone")
            };
        }

        public Level GetLevel(WorldID worldId, int level)
        {
            //Set to the nexus for default
            Level returnLevel = GameLevels[3];

            foreach (var item in GameLevels)
            {
                if (item.World == worldId && item.LevelID == level) returnLevel = item;
            }

            return returnLevel;
        }
    }
}
