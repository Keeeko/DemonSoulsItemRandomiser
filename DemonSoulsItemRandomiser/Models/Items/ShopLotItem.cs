using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models
{
    public class ShopLotItem : Item
    {
        public ShopLotItem OwnerShopLot;
        public ItemShopCategoryID EquipType { get; set; }
        public long EquipID { get; set; }
        public string Description { get; set; }

        public ShopLotItem(ShopLot ownerShopLot, ItemShopCategoryID equipType, long equipId ) : base(equipId, equipType.ShopCategoryToNormalCategory())
        {
            EquipType = equipType;
            EquipID = equipId;
        }

        public override string ToString()
        {
            return EquipID + " - " + Description;
        }
    }
}
