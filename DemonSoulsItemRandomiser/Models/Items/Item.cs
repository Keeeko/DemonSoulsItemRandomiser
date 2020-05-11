using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class Item
    {
        public long ID { get; set; }
        public ItemCategoryID ItemCategory { get; set; }
        public bool KeyItem { get; set; }

        public Item(long iD, ItemCategoryID itemCategory)
        {
            ID = iD;
            ItemCategory = itemCategory;

            if (IDBanks.keyItems.Contains(ID)) KeyItem = true;
            else KeyItem = false;
        }
    }
}
