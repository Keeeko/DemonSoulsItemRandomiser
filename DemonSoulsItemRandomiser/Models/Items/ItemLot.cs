using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class ItemLot : ISaveable
    {
        public long ID { get; set; }
        public int Rarity { get; set; }
        public string Description { get; set; }
        public long EventID { get; set; }

        public List<ItemLotItem> ItemLotItems { get; set; }
        public ItemLotType LotType { get; set; }
        public PARAM.Row OriginalParamRow { get; set; }
   
        public ItemLot(PARAM.Row itemLotRow)
        {
            ItemLotItems = new List<ItemLotItem>();
            ID = itemLotRow.ID;
            Rarity = Convert.ToInt32(itemLotRow["lotItem_Rarity"].Value);
            Description = itemLotRow.Name;
            EventID = Convert.ToInt64(itemLotRow["eventId"].Value);
            OriginalParamRow = itemLotRow;

            for (int i = 1; i < 10; i++)
            {
                if(i != 9)
                {
                    ItemLotItems.Add(new ItemLotItem((ItemCategoryID) itemLotRow["lotItemCategory0" + i].Value, Convert.ToInt64(itemLotRow["lotItemId0" + i].Value), Convert.ToInt32(itemLotRow["lotItemNum0" + i].Value), Convert.ToInt32(itemLotRow["lotItemBasePoint0" + i].Value),
                        Convert.ToInt32(itemLotRow["QWCBasePoint0" + i].Value), Convert.ToInt32(itemLotRow["QWCAppliesPoint0" + i].Value), Convert.ToBoolean(itemLotRow["enableLuck0" + i].Value), this));
                }
                else
                {
                    ItemLotItems.Add(new ItemLotItem((ItemCategoryID)itemLotRow["hostOnlyItemCate"].Value, Convert.ToInt64(itemLotRow["hostOnlyItemId"].Value), Convert.ToInt32(itemLotRow["hostOnlyItemNum"].Value), Convert.ToInt64(itemLotRow["eventId"].Value), this));
                }
            }
        }

        public void SwapItemLotValues(ItemLot itemToSwapWith)
        {
            List<ItemLotItem> myOriginalItemLotItems = ItemLotItems;
            List<ItemLotItem> itemToSwapWithOriginalLotItems = itemToSwapWith.ItemLotItems;

            //Swap
            ItemLotItems = itemToSwapWithOriginalLotItems;
            itemToSwapWith.ItemLotItems = myOriginalItemLotItems;
        }

        public void Save()
        {
            OriginalParamRow["lotItem_Rarity"].Value = Rarity;
            OriginalParamRow["eventId"].Value = EventID;

            for (int i = 1; i < 10; i++)
            {
                if (i != 9)
                {
                    OriginalParamRow["lotItemCategory0" + i].Value = ItemLotItems.First().LotItemCategory;
                    OriginalParamRow["lotItemId0" + i].Value = ItemLotItems.First().LotItemId;
                    OriginalParamRow["lotItemNum0" + i].Value = ItemLotItems.First().LotItemNumber;
                    OriginalParamRow["lotItemBasePoint0" + i].Value = ItemLotItems.First().LotItemBasePoint;
                    OriginalParamRow["QWCBasePoint0" + i].Value = ItemLotItems.First().QWCBasePoint;
                    OriginalParamRow["QWCAppliesPoint0" + i].Value = ItemLotItems.First().QWCAppliesPoint;
                    OriginalParamRow["enableLuck0" + i].Value = ItemLotItems.First().EnableLuck;
                }
                else
                {
                    OriginalParamRow["hostOnlyItemCate"].Value = ItemLotItems.Last().HostOnlyItemCategory;
                    OriginalParamRow["hostOnlyItemId"].Value = ItemLotItems.Last().HostOnlyItemID;
                    OriginalParamRow["hostOnlyItemNum"].Value = ItemLotItems.Last().HostOnlyItemNumber;
                }
            }
        }

        public override string ToString()
        {
            return ID + " - " + Description;
        }
    }
}
