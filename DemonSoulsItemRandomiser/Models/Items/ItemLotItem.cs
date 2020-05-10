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
        //Host only item (Generally NPC Drops)
        public bool HostOnlyItem { get; set; }
        public long HostOnlyItemCategory { get; set; }
        public long HostOnlyItemID { get; set; }
        public int  HostOnlyItemNumber { get; set; }
        public long EventId { get; set; }

        //Lot Item (World drops, enemy drops and other interactable objects)
        public long LotItemCategory { get; set; }
        public long LotItemId { get; set; }
        public int  LotItemNumber { get; set; }
        public int  LotItemBasePoint { get; set; }
        public int  QWCBasePoint { get; set; }
        public int  QWCAppliesPoint { get; set; }
        public bool EnableLuck { get; set; }

        public ItemLotItem(long hostOnlyItemCategory, long hostOnlyItemID, int hostOnlyItemNumber, long eventId)
        {
            HostOnlyItemCategory = hostOnlyItemCategory;
            HostOnlyItemID = hostOnlyItemID;
            HostOnlyItemNumber = hostOnlyItemNumber;
            EventId = eventId;
            HostOnlyItem = true;
        }

        public ItemLotItem(long lotItemCategory, long lotItemId, int lotItemNumber, int lotItemBasePoint, int qWCBasePoint, int qWCAppliesPoint, bool enableLuck)
        {
            LotItemCategory = lotItemCategory;
            LotItemId = lotItemId;
            LotItemNumber = lotItemNumber;
            LotItemBasePoint = lotItemBasePoint;
            QWCBasePoint = qWCBasePoint;
            QWCAppliesPoint = qWCAppliesPoint;
            EnableLuck = enableLuck;
            HostOnlyItem = false;
        }

        public override string ToString()
        {
            if (HostOnlyItem) return HostOnlyItemID.ToString();
            else return LotItemId.ToString();
        }
    }
}
