using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class ItemShop : ISaveable
    {
        public NPCShopkeeper ownerShopKeeper;
        PARAM.Row originalRow;

        public long ID { get; set; }
        public ShopTypeID ShopType { get; set; }
        public ItemShopCategoryID EquipType { get; set; }
        public long EquipID { get; set; }
        public int Value { get; set; }
        public int MaterialID { get; set; }
        public long EventFlag { get; set; }
        public int SellQuantity { get; set; }
        public long QwcID { get; set; }
        public string Description { get; set; }

        public ItemShop(PARAM.Row itemShop, NPCShopkeeper owner)
        {
            ID =              itemShop.ID;
            ShopType =        (ShopTypeID) Convert.ToInt32(itemShop["shopType"].Value);
            EquipType =       (ItemShopCategoryID) Convert.ToInt32(itemShop["equipType"].Value);
            EquipID =         Convert.ToInt64(itemShop["equipId"].Value);
            Value =           Convert.ToInt32(itemShop["value"].Value);
            MaterialID =      Convert.ToInt32(itemShop["mtrlId"].Value);
            EventFlag =       Convert.ToInt32(itemShop["eventFlag"].Value);
            SellQuantity =    Convert.ToInt32(itemShop["sellQuantity"].Value);
            QwcID =           Convert.ToInt64(itemShop["qwcId"].Value);
            Description =     itemShop.Name;
            ownerShopKeeper = owner;
            originalRow =     itemShop;
        }

        public void Save()
        {
            originalRow["shopType"].Value = (int)ShopType;
            originalRow["equipType"].Value = (int)EquipType;
            originalRow["value"].Value = Value;
            originalRow["mtrlId"].Value = MaterialID;
            originalRow["eventFlag"].Value = EventFlag;
            originalRow["sellQuantity"].Value = SellQuantity;
            originalRow["qwcId"].Value = QwcID;
        }

        public override string ToString()
        {
            return ownerShopKeeper + " - " + EquipID;
        }
    }
}
