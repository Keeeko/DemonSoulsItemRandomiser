using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DemonSoulsItemRandomiser.Models.Items
{
    public class ItemShop
    {
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

        public ItemShop(PARAM.Row itemShop)
        {
            
        }
    }
}
