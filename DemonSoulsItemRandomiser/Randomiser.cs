using DemonSoulsItemRandomiser.Models;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser
{
    public class Randomiser
    {
        public bool RandomiseWorldTreasure { get; set; }
        public bool RandomiseKeyItems { get; set; }
        public bool RandomiseStartingEquipment { get; set; }
        public bool RandomiseEnemyDropTables { get; set; }
        public bool RandomiseShopInventory { get; set; }
        public bool UseRandomSeed { get; set; }
        public int RandomSeed { get; set; }

        Random rng;
        public Randomiser()
        {
            RandomiseWorldTreasure = true;
            rng = new Random();
            RandomSeed = rng.Next(0, 2_147_483_647);
        }

        public void RandomiseItems()
        {
            Random rng;
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

        private void RandomiseEnemyLots()
        {
            foreach (var item in MainWindow.GameWorld.enemyDropTableRows)
            {
                //8 is the number of drop table slots
                for (int i = 1; i < 8; i++)
                {
                    //Find non-empty drop table slot
                    if (Convert.ToInt64(item["lotItemId0" + i].Value) != 0 && Convert.ToInt64(item["lotItemCategory0" + i].Value) != -1)
                    {
                        //Find a random item with a valid drop table slot
                        PARAM.Row rowToSwapWith = MainWindow.GameWorld.enemyDropTableRows[rng.Next(MainWindow.GameWorld.enemyDropTableRows.Count)];
                        while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId0" + i].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory0" + i].Value) == -1)
                        {
                            rowToSwapWith = MainWindow.GameWorld.enemyDropTableRows[rng.Next(MainWindow.GameWorld.enemyDropTableRows.Count)];
                        }

                        //Swap the droptable slots
                        SwapItemValues(item, rowToSwapWith, "lotItemId0" + i);
                        SwapItemValues(item, rowToSwapWith, "lotItemNum0" + i);
                        SwapItemValues(item, rowToSwapWith, "lotItemBasePoint0" + i);
                        SwapItemValues(item, rowToSwapWith, "QWCBasePoint0" + i);
                        SwapItemValues(item, rowToSwapWith, "QWCAppliesPoint0" + i);
                        SwapItemValues(item, rowToSwapWith, "enableLuck0" + i);
                        SwapItemValues(item, rowToSwapWith, "lotItemCategory0" + i);
                    }
                }
            }
        }

        private void RandomiseShopItems()
        {
            foreach (var item in MainWindow.GameWorld.shopItemRows)
            {
                //We do not want to potentially reinsert key items that have been swapped into the shop back into the world at forbidden id's
                if (!IDBanks.keyItems.Contains(item.ID))
                {
                    //Decide if swapping with vendor or with world items
                    bool swapWithWorldItem = false;

                    swapWithWorldItem = rng.Next(0, 2) > 0;

                    if (swapWithWorldItem)
                    {
                        //Pick a random treasure to swap with, make sure its not a NORMAL soul drop
                        PARAM.Row treasureItemToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];
                        while (Convert.ToInt64(treasureItemToSwapWith["lotItemCategory01"].Value) == -1 && !IDBanks.soulsIds.Contains(Convert.ToInt64(treasureItemToSwapWith["lotItemId01"].Value))) treasureItemToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];

                        var treasureItemToSwapWithItemId = treasureItemToSwapWith["lotItemId01"].Value;
                        var treasureItemToSwapWithCategory = 0;

                        var currentRowItemId = item["equipId"].Value;
                        var currentRowItemCategory = 0;

                        switch (Convert.ToInt64(item["equipType"].Value))
                        {
                            case 0:
                                currentRowItemCategory = 0;
                                break;
                            case 1:
                                currentRowItemCategory = 268435456;
                                break;
                            case 2:
                                currentRowItemCategory = 536870912;
                                break;
                            case 3:
                                currentRowItemCategory = 1073741824;
                                break;
                        }

                        switch (Convert.ToInt64(treasureItemToSwapWith["lotItemCategory01"].Value))
                        {
                            case 0:
                                treasureItemToSwapWithCategory = 0;
                                break;
                            case 268435456:
                                treasureItemToSwapWithCategory = 1;
                                break;
                            case 536870912:
                                treasureItemToSwapWithCategory = 2;
                                break;
                            case 1073741824:
                                treasureItemToSwapWithCategory = 3;
                                break;
                        }

                        treasureItemToSwapWith["lotItemId01"].Value = currentRowItemId;
                        treasureItemToSwapWith["lotItemCategory01"].Value = currentRowItemCategory;

                        //Make sure we don't end up creating a stacked drop of equipment
                        if (currentRowItemCategory != 1073741824) treasureItemToSwapWith["lotItemNum01"].Value = 1;

                        item["equipId"].Value = treasureItemToSwapWithItemId;
                        item["equipType"].Value = treasureItemToSwapWithCategory;

                        //Make sure infinite souls cant be purchased from shop
                        if (IDBanks.bossItems.Contains(Convert.ToInt64(item["equipId"].Value)) || IDBanks.soulsIds.Contains(Convert.ToInt64(item["equipId"].Value))) item["sellQuantity"].Value = 1;
                    }

                    if (!swapWithWorldItem)
                    {
                        //Don't swap an item with itself
                        PARAM.Row rowToSwapWith = MainWindow.GameWorld.shopItemRows[rng.Next(MainWindow.GameWorld.shopItemRows.Count)];
                        while (rowToSwapWith.ID == item.ID) rowToSwapWith = MainWindow.GameWorld.shopItemRows[rng.Next(MainWindow.GameWorld.shopItemRows.Count)];

                        SwapItemValues(item, rowToSwapWith, "equipType");
                        SwapItemValues(item, rowToSwapWith, "equipId");
                        SwapItemValues(item, rowToSwapWith, "value");
                        SwapItemValues(item, rowToSwapWith, "mtrlId");
                        SwapItemValues(item, rowToSwapWith, "eventFlag");
                        SwapItemValues(item, rowToSwapWith, "sellQuantity");
                        SwapItemValues(item, rowToSwapWith, "qwcId");
                    }
                }
            }
        }

        private void RandomiseKeyItemLots()
        {
            foreach (var item in MainWindow.GameWorld.keyItemLotRows)
            {
                PARAM.Row rowToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];
                while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1 || IDBanks.forbiddenItemLotIds.Contains(rowToSwapWith.ID)) rowToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];

                SwapItemValues(item, rowToSwapWith, "lotItemId01");
                SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
                SwapItemValues(item, rowToSwapWith, "lotItemNum01");
            }
        }

        private void RandomiseBossSouls()
        {
            foreach (var item in MainWindow.GameWorld.bossItemsRows)
            {
                //Find non-empty drop table slot
                if (Convert.ToInt64(item["lotItemId01"].Value) != 0 && Convert.ToInt64(item["lotItemCategory01"].Value) != -1)
                {
                    //Find a random item with a valid drop table slot
                    PARAM.Row rowToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];
                    while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId01"].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1)
                    {
                        rowToSwapWith = MainWindow.GameWorld.enemyDropTableRows[rng.Next(MainWindow.GameWorld.enemyDropTableRows.Count)];
                    }

                    //Swap the droptable slots
                    SwapItemValues(item, rowToSwapWith, "lotItemId01");
                    SwapItemValues(item, rowToSwapWith, "lotItemNum01");
                    SwapItemValues(item, rowToSwapWith, "lotItemBasePoint01");
                    SwapItemValues(item, rowToSwapWith, "QWCBasePoint01");
                    SwapItemValues(item, rowToSwapWith, "QWCAppliesPoint01");
                    SwapItemValues(item, rowToSwapWith, "enableLuck01");
                    SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
                }
                else if (Convert.ToInt64(item["lotItemId02"].Value) != 0 && Convert.ToInt64(item["lotItemCategory02"].Value) != -1)
                {
                    //Find a random item from treasure drops
                    PARAM.Row rowToSwapWith = MainWindow.GameWorld.treasureItemLotsRows[rng.Next(MainWindow.GameWorld.treasureItemLotsRows.Count)];
                    while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId01"].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1)
                    {
                        rowToSwapWith = MainWindow.GameWorld.enemyDropTableRows[rng.Next(MainWindow.GameWorld.enemyDropTableRows.Count)];
                    }

                    //Swap the boss soul drops
                    SwapItemValues(item, rowToSwapWith, "lotItemId02", "lotItemId01");
                    SwapItemValues(item, rowToSwapWith, "lotItemNum02", "lotItemNum01");
                    SwapItemValues(item, rowToSwapWith, "lotItemBasePoint02", "lotItemBasePoint01");
                    SwapItemValues(item, rowToSwapWith, "QWCBasePoint02", "QWCBasePoint01");
                    SwapItemValues(item, rowToSwapWith, "QWCAppliesPoint02", "QWCAppliesPoint01");
                    SwapItemValues(item, rowToSwapWith, "enableLuck02", "enableLuck01");
                    SwapItemValues(item, rowToSwapWith, "lotItemCategory02", "lotItemCategory01");
                }
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

        private void SwapItemValues(PARAM.Row val1, PARAM.Row val2, string valueName)
        {
            var original = val1[valueName].Value;
            var newVal = val2[valueName].Value;

            val1[valueName].Value = newVal;
            val2[valueName].Value = original;
        }

        private void SwapItemValues(PARAM.Row val1, PARAM.Row val2, string valueName, string valueName2)
        {
            var original = val1[valueName].Value;
            var newVal = val2[valueName2].Value;

            val1[valueName].Value = newVal;
            val2[valueName2].Value = original;
        }

        private bool VerifyItemLot(PARAM.Row item)
        {
            if (IDBanks.weaponIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
            {
                if (Convert.ToInt64(item["lotItemCategory01"].Value) != 0) return false;
            }

            if (IDBanks.armorIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
            {
                if (Convert.ToInt64(item["lotItemCategory01"].Value) != 268435456) return false;
            }

            if (IDBanks.accesoryIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
            {
                if (Convert.ToInt64(item["lotItemCategory01"].Value) != 536870912) return false;
            }

            if (MainWindow.GameWorld.consumeablesIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
            {
                if (Convert.ToInt64(item["lotItemCategory01"].Value) != 1073741824) return false;
            }

            return true;
        }
    }
}
