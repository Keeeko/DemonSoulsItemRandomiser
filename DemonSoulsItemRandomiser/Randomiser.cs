using DemonSoulsItemRandomiser.Models;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser 
{
    public class Randomiser : INotifyPropertyChanged
    {
        public bool RandomiseWorldTreasure { get; set; }
        public bool RandomiseKeyItems { get; set; }
        public bool RandomiseStartingEquipment { get; set; }
        public bool RandomiseEnemyDropTables { get; set; }
        public bool RandomiseShopInventory { get; set; }
        public bool UseRandomSeed { get; set; }
        public int RandomSeed { get; set; }

        Random rng;

        public event PropertyChangedEventHandler PropertyChanged;

        public Randomiser()
        {
            RandomiseWorldTreasure = true;
            rng = new Random();
            RandomSeed = rng.Next(0, 2_147_483_647);
        }

        public void RandomiseItems()
        {
            if (UseRandomSeed) rng = new Random(RandomSeed);
            else rng = new Random();

            //Swapping treasure
            if (RandomiseWorldTreasure) RandomiseTreasureLots();

            //Swapping EnemyDrop tables
            if (RandomiseEnemyDropTables) RandomiseEnemyLots();

            //Swapping key items
            if (RandomiseKeyItems) RandomiseKeyItemLots();

            //Swapping shop inventory
            if (RandomiseShopInventory) RandomiseShopItems();

            //Swapping starting equipment
            if (RandomiseStartingEquipment) RandomiseStartingEquipmentItems();

            //Randomise Boss souls
            RandomiseBossSouls();

            //Save all the randomisation we have done
            MainWindow.GameWorld.SaveChangesToData();
        }

        private void RandomiseTreasureLots()
        {
            List<ItemLot> cachedTreasureLots = MainWindow.GameWorld.GetAllTreasureLots();

            foreach (var item in cachedTreasureLots)
            {
                if ((int) item.ItemLotItems[0].LotItemCategory != -1)
                {
                    ItemLot itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];
                    while (itemToSwapWith.ID == item.ID || (int) itemToSwapWith.ItemLotItems[0].LotItemCategory == -1) itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];

                    item.SwapItemLotValues(itemToSwapWith);
                }
            }
        }

        //This needs to be able to swap individual drops and their chance of dropping around instead of the whole drop table to an enemy
        private void RandomiseEnemyLots()
        {
            List<ItemLot> cachedEnemyLots = MainWindow.GameWorld.GetAllEnemyItemLots();

            foreach (var item in cachedEnemyLots)
            {
                ItemLot itemToSwapWith = cachedEnemyLots[rng.Next(cachedEnemyLots.Count)];

                while (itemToSwapWith.ID == item.ID)
                {
                    itemToSwapWith = cachedEnemyLots[rng.Next(cachedEnemyLots.Count)];
                }

                item.SwapItemLotValues(itemToSwapWith);
            }
        }

        private void RandomiseShopItems()
        {
            List<ShopLot> cachedShopItems = MainWindow.GameWorld.GetAllShopItems();
            List<ItemLot> cachedTreasureLots = MainWindow.GameWorld.GetAllTreasureLots();

            foreach (var shopLot in cachedShopItems)
            {
                //We do not want to potentially reinsert key items that have been swapped into the shop back into the world at forbidden id's
                if (!IDBanks.keyItems.Contains(shopLot.ShopLotItem.EquipID))
                {
                    //Decide if swapping with vendor or with world items
                    bool swapWithWorldItem = true;

                    //swapWithWorldItem = rng.Next(0, 2) > 0;

                    if (swapWithWorldItem)
                    {
                        //Pick a random treasure to swap with, make sure its not a NORMAL soul drop
                        ItemLot treasureItemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];
                        while (shopLot.ownerShopKeeper.HasShopLotItem(treasureItemToSwapWith) || Convert.ToInt64(treasureItemToSwapWith.ItemLotItems[0].ItemCategory) == -1 || IDBanks.soulsIds.Contains(treasureItemToSwapWith.ItemLotItems[0].ID))
                            treasureItemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];

                        shopLot.SwapItemLotValues(treasureItemToSwapWith.ItemLotItems[0]);
                    }

                    if (!swapWithWorldItem)
                    {
                        //Don't swap an item with itself
                        ShopLot shopItemToSwapWith = cachedShopItems[rng.Next(cachedShopItems.Count)];
                        while (shopItemToSwapWith.ID == shopLot.ID || shopLot.ownerShopKeeper.HasShopLotItem(shopItemToSwapWith.ShopLotItem)) shopItemToSwapWith = cachedShopItems[rng.Next(cachedShopItems.Count)];

                        shopLot.SwapItemLotValues(shopItemToSwapWith.ShopLotItem);
                    }
                }
            }
        }

        private void RandomiseKeyItemLots()
        {
            List<ItemLot> cachedTreasureLots = MainWindow.GameWorld.GetAllTreasureLots();
            List<ItemLot> cachedKeyLots = MainWindow.GameWorld.GetAllKeyItems();

            foreach (var item in cachedKeyLots)
            {
                ItemLot itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];
                while (itemToSwapWith.ID == item.ID || (int) itemToSwapWith.ItemLotItems.First().ItemCategory == -1 || IDBanks.forbiddenItemLotIds.Contains(itemToSwapWith.ID)) itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];

                item.SwapItemLotValues(itemToSwapWith);
            }
        }

        private void RandomiseBossSouls()
        {
            List<ItemLot> cachedBossSouls = MainWindow.GameWorld.GetAllBossSouls();
            List<ItemLot> cachedTreasureLots = MainWindow.GameWorld.GetAllTreasureLots();

                foreach (var item in cachedBossSouls)
                {
                    ItemLot itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];
                    while (itemToSwapWith.ID == item.ID || (int)itemToSwapWith.ItemLotItems.First().ItemCategory == -1 || IDBanks.forbiddenItemLotIds.Contains(itemToSwapWith.ID)) itemToSwapWith = cachedTreasureLots[rng.Next(cachedTreasureLots.Count)];

                    item.SwapItemLotValues(itemToSwapWith);
                }
        }

        private void RandomiseStartingEquipmentItems()
        {
            //foreach (var item in startingEquipmentRows)
            //{
            //    PARAM.Row rowToSwapWith = weaponDatabase[rng.Next(weaponDatabase.Count)];
            //    while (rowToSwapWith.ID == item.ID) rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

            //    SwapItemValues(item, rowToSwapWith, "lotItemId01");
            //    SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
            //    SwapItemValues(item, rowToSwapWith, "lotItemNum01");

            //}
        }
    }
}
