using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemonSoulsItemRandomiser.Models;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class NPCShopkeeper
    {
        public string Name { get; set; }
        public List<ShopLot> Items {get; set;}
        public Level OwnerLevel { get; set; }

        public NPCShopkeeper(string name, Level ownerLevel)
        {
            Name = name;
            Items = new List<ShopLot>();
            OwnerLevel = ownerLevel;
        }

        public bool HasShopLotItem(ShopLotItem shopItem)
        {
            foreach (var item in Items)
            {
                if (item.ShopLotItem.ID == shopItem.ID && item.ShopLotItem.ItemCategory == shopItem.ItemCategory) return true;
            }

            return false;
        }

        public bool HasShopLotItem(ItemLot itemLot)
        {
            foreach (var item in Items)
            {
                if (item.ShopLotItem.ID == itemLot.ItemLotItems[0].ID && item.ShopLotItem.EquipType.ShopCategoryToNormalCategory() == itemLot.ItemLotItems[0].ItemCategory) return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
