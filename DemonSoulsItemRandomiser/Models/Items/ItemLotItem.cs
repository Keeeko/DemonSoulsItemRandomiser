using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class ItemLotItem
    {
        ItemLot ParentItemLot { get; set; }
        public string ItemDescription { get; set; }

        //Host only item (Generally NPC Drops)
        private bool HostOnlyItem { get; set; }
        public ItemCategoryID HostOnlyItemCategory { get; set; }
        public long HostOnlyItemID { get; set; }
        public int  HostOnlyItemNumber { get; set; }

        //Lot Item (World drops, enemy drops and other interactable objects)
        public ItemCategoryID LotItemCategory { get; set; }
        public long LotItemId { get; set; }
        public int  LotItemNumber { get; set; }
        public int  LotItemBasePoint { get; set; }
        public int  QWCBasePoint { get; set; }
        public int  QWCAppliesPoint { get; set; }
        public bool EnableLuck { get; set; }

        public ItemLotItem(ItemCategoryID hostOnlyItemCategory, long hostOnlyItemID, int hostOnlyItemNumber, long eventId, ItemLot parentItemLot)
        {
            HostOnlyItemCategory = hostOnlyItemCategory;
            HostOnlyItemID = hostOnlyItemID;
            HostOnlyItemNumber = hostOnlyItemNumber;
            HostOnlyItem = true;
            ParentItemLot = parentItemLot;
            
            SetItemDescription();
        }

        public ItemLotItem(ItemCategoryID lotItemCategory, long lotItemId, int lotItemNumber, int lotItemBasePoint, int qWCBasePoint, int qWCAppliesPoint, bool enableLuck, ItemLot parentItemLot)
        {
            LotItemCategory = lotItemCategory;
            LotItemId = lotItemId;
            LotItemNumber = lotItemNumber;
            LotItemBasePoint = lotItemBasePoint;
            QWCBasePoint = qWCBasePoint;
            QWCAppliesPoint = qWCAppliesPoint;
            EnableLuck = enableLuck;
            HostOnlyItem = false;
            ParentItemLot = parentItemLot;

            SetItemDescription();
        }

        private void SetItemDescription()
        {
            //Disabled, this needs optimised or predone then cached into the original data file as opening up connections for each items is wrecking application performance
            //ItemDescription = Util.ItemDescriptionTranslator.TranslateItem(HostOnlyItem ? HostOnlyItemCategory : LotItemCategory, HostOnlyItem ? HostOnlyItemID : LotItemId);
        }

        public override string ToString()
        {
            return HostOnlyItem ? HostOnlyItemID.ToString() : LotItemId.ToString() + " - " + ItemDescription;
        }
    }
}
