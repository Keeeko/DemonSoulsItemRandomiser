using DemonSoulsItemRandomiser.Models;
using System.Collections.Generic;

namespace DemonSoulsItemRandomiser
{
    public interface IItemLotCollectionOwner
    {
        List<ItemLot> GetAllItemLots();
    }
}
