using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemonSoulsItemRandomiser.Models;

namespace DemonSoulsItemRandomiser.Util
{
    public static class ItemLotRetrievalFilters
    {
        public static List<ItemLot> GetTreasureItemLots(List<ItemLot> inputItemLots)
        {
            List<ItemLot> treasureItemLots = new List<ItemLot>();

            foreach (var item in inputItemLots)
            {
                if(IDBanks.treasureItemLots.Contains(item.ID))
                {
                    treasureItemLots.Add(item);
                }
            }

            return treasureItemLots;
        }

        public static List<ItemLot> GetEnemyItemLots(List<ItemLot> inputItemLots)
        {
            List<ItemLot> enemyItemLots = new List<ItemLot>();

            foreach (var item in inputItemLots)
            {
                if (IDBanks.enemyDropTableLotIds.Contains(item.ID))
                {
                    enemyItemLots.Add(item);
                }
            }

            return enemyItemLots;
        }
    }
}
