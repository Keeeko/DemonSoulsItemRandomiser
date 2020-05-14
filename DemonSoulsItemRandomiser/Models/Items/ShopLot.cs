using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class ShopLot : ISaveable
    {
        public long ID { get; set; }

        public NPCShopkeeper ownerShopKeeper;
        PARAM.Row originalRow;
        public ShopTypeID ShopType { get; set; }
        public ShopLotItem ShopLotItem { get; set; }
        public int Value { get; set; }
        public int MaterialID { get; set; }
        public long EventFlag { get; set; }
        public int SellQuantity { get; set; }
        public long QwcID { get; set; }
        public string Description { get; set; }

        public ShopLot(PARAM.Row itemShop, NPCShopkeeper owner)
        {
            ID = itemShop.ID;
            ShopLotItem = new ShopLotItem(this, (ItemShopCategoryID) Convert.ToInt64(itemShop["equipType"].Value), Convert.ToInt64(itemShop["equipId"].Value));
            ShopType = (ShopTypeID)Convert.ToInt32(itemShop["shopType"].Value);
            Value = Convert.ToInt32(itemShop["value"].Value);
            MaterialID = Convert.ToInt32(itemShop["mtrlId"].Value);
            EventFlag = Convert.ToInt32(itemShop["eventFlag"].Value);
            SellQuantity = Convert.ToInt32(itemShop["sellQuantity"].Value);
            QwcID = Convert.ToInt64(itemShop["qwcId"].Value);
            Description = itemShop.Name;
            ownerShopKeeper = owner;
            originalRow = itemShop;
        }

        public void SwapItemLotValues(ItemLotItem itemToSwapWith)
        {
            var myOriginalItemCategory = ShopLotItem.EquipType;
            var myOriginalItemID = ShopLotItem.ID;

            var itemLotToSwapWithOriginalItemCategory = itemToSwapWith.ItemCategory;
            var itemLotToSwapWithOriginalItemId = itemToSwapWith.ID;

            var itemLotToSwapWithNewItemCategory = itemToSwapWith.ItemCategory;
            var myNewItemCategory = ShopLotItem.EquipType;

            //Swap ID's
            ShopLotItem.EquipID = itemLotToSwapWithOriginalItemId;
            itemToSwapWith.ID = myOriginalItemID;

            switch (itemToSwapWith.LotItemCategory)
            {
                case ItemCategoryID.WEAPON:
                    myNewItemCategory = ItemShopCategoryID.WEAPON;
                    break;
                case ItemCategoryID.ARMOR:
                    myNewItemCategory = ItemShopCategoryID.ARMOR;
                    break;
                case ItemCategoryID.ACCESORIES:
                    myNewItemCategory = ItemShopCategoryID.ACCESORY;
                    break;
                case ItemCategoryID.CONSUMABLES:
                    myNewItemCategory = ItemShopCategoryID.CONSUMEABLE;
                    break;
                default:
                    break;
            }

            switch (ShopLotItem.EquipType)
            {
                case ItemShopCategoryID.WEAPON:
                    itemLotToSwapWithNewItemCategory = ItemCategoryID.WEAPON;
                    break;
                case ItemShopCategoryID.ARMOR:
                    itemLotToSwapWithNewItemCategory = ItemCategoryID.ARMOR;
                    break;
                case ItemShopCategoryID.ACCESORY:
                    itemLotToSwapWithNewItemCategory = ItemCategoryID.ACCESORIES;
                    break;
                case ItemShopCategoryID.CONSUMEABLE:
                    itemLotToSwapWithNewItemCategory = ItemCategoryID.CONSUMABLES;
                    break;
                case ItemShopCategoryID.SORCERY:
                    Console.WriteLine("Swapping magic spells, this is most likely a mistake.");
                    break;
                case ItemShopCategoryID.MIRACLE:
                    Console.WriteLine("Swapping magic spells, this is most likely a mistake.");
                    break;
                default:
                    break;
            }

            //Swap Categories
            ShopLotItem.EquipType = myNewItemCategory;
            itemToSwapWith.ItemCategory = itemLotToSwapWithNewItemCategory;

            //Make sure we don't end up creating a stacked drop of equipment
            if (itemToSwapWith.ItemCategory != ItemCategoryID.CONSUMABLES) itemToSwapWith.LotItemNumber = 1;

            //Make sure infinite souls cant be purchased from shop
            if (IDBanks.bossItems.Contains(ShopLotItem.EquipID) || IDBanks.soulsIds.Contains(ShopLotItem.EquipID)) SellQuantity = 1;
        }

        public void SwapItemLotValues(ShopLotItem itemToSwapWith)
        {
            ShopLotItem myOriginalShopLotItem = ShopLotItem;
            ShopLotItem itemToSwapWithOriginalShopItem = itemToSwapWith;

            //Swap
            ShopLotItem = itemToSwapWithOriginalShopItem;
            itemToSwapWith = myOriginalShopLotItem;
        }

        public void Save()
        {
            originalRow["shopType"].Value = (int) ShopType;
            originalRow["equipId"].Value = (long)ShopLotItem.EquipID;
            originalRow["equipType"].Value = (long) ShopLotItem.EquipType;
            originalRow["value"].Value = Value;
            originalRow["mtrlId"].Value = MaterialID;
            originalRow["eventFlag"].Value = EventFlag;
            originalRow["sellQuantity"].Value = SellQuantity;
            originalRow["qwcId"].Value = QwcID;
        }

        public override string ToString()
        {
            return ownerShopKeeper + " - " + ID;
        }
    }
}
