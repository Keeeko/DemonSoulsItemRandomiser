using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using SoulsFormats;
using System.ComponentModel;
using System.Diagnostics;
using DemonSoulsItemRandomiser.Models;

namespace DemonSoulsItemRandomiser
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        readonly string pathToParamDataFile = Directory.GetCurrentDirectory() + @"\Data\gameparamna.parambnd.dcx";
        readonly string pathToParamDef = Directory.GetCurrentDirectory() + @"\Data\paramdef\paramdef.paramdefbnd.dcx";

        public event PropertyChangedEventHandler PropertyChanged;

        public bool RandomiseWorldTreasure { get; set; }
        public bool RandomiseKeyItems { get; set; }
        public bool RandomiseStartingEquipment { get; set; }
        public bool RandomiseEnemyDropTables { get; set; }
        public bool RandomiseShopInventory { get; set; }
        public bool UseRandomSeed { get; set; }
        public int RandomSeed { get; set; }

        public static Dictionary<string, PARAMDEF> paramDefs;
        public static BND3 paramDefBnd;

        public static Dictionary<string, PARAM> parms;
        public static BND3 paramBnd;

        //The orbs enemy drop after they die
        List<PARAM.Row> enemyDropTableRows;

        //World Drops (Glowing orbs on map)
        List<PARAM.Row> treasureItemLotsRows;

        //Items that allow progression
        List<PARAM.Row> keyItemLotRows;

        //Items held in shops
        List<PARAM.Row> shopItemRows;

        //Boss items
        List<PARAM.Row> bossItemsRows;

        //Starting Characters
        List<PARAM.Row> startingEquipmentRows;

        //Weapons
        List<PARAM.Row> weaponDatabase;

        //Weapons
        List<PARAM.Row> armorDatabase;

        //Accesories
        List<PARAM.Row> accesoryDatabase;

        //Souls items
        List<PARAM.Row> soulsDatabase;

        //Consumeables
        List<long> consumeablesIds;

        public GameWorld GameWorld { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            RandomiseWorldTreasure = true;
            Random rng = new Random();
            RandomSeed = rng.Next(0, 2_147_483_647);

            GameWorld = new GameWorld();
            IDBanks.InitIDLists();
        }

        private void UnpackGameBNDFile()
        {
            // Reading an original paramdefbnd
            paramDefs = new Dictionary<string, PARAMDEF>();
            paramDefBnd = BND3.Read(pathToParamDef);

            foreach (BinderFile file in paramDefBnd.Files)
            {
                var paramdef = PARAMDEF.Read(file.Bytes);
                paramDefs[paramdef.ParamType] = paramdef;
            }

            parms = new Dictionary<string, PARAM>();
            var parambnd = BND3.Read(pathToParamDataFile);

            foreach (BinderFile file in parambnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                var param = PARAM.Read(file.Bytes);
                param.ApplyParamdef(paramDefs[param.ParamType]);
                parms[name] = param;
            }
        }

        private void RepackGameBNDFile()
        {

            foreach (BinderFile file in paramBnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                file.Bytes = parms[name].Write();
            }

            paramBnd.Write(pathToParamDataFile);
        }

        private void RandomiseItems()
        {

            UnpackGameBNDFile();
            InitRowLists();

            Random rng;
            if (UseRandomSeed) rng = new Random(RandomSeed);
            else  rng = new Random();


            //Swapping treasure
            if (RandomiseWorldTreasure)
            {
                foreach (var item in treasureItemLotsRows)
                {
                    if(Convert.ToInt64(item["lotItemCategory01"].Value) != -1)
                    {

                        PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                        while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1) rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

                        SwapItemValues(item, rowToSwapWith, "lotItemId01");
                        SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
                        SwapItemValues(item, rowToSwapWith, "lotItemNum01");
                    }
                }
            }


            //Swapping EnemyDrop tables
            if (RandomiseEnemyDropTables)
            {
                foreach (var item in enemyDropTableRows)
                {
                    //8 is the number of drop table slots
                    for (int i = 1; i < 8; i++)
                    {
                        //Find non-empty drop table slot
                        if(Convert.ToInt64(item["lotItemId0" + i].Value) != 0 && Convert.ToInt64(item["lotItemCategory0" + i].Value) != -1)
                        {
                            //Find a random item with a valid drop table slot
                            PARAM.Row rowToSwapWith = enemyDropTableRows[rng.Next(enemyDropTableRows.Count)];
                            while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId0" + i].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory0" + i].Value) == -1)
                            {
                                rowToSwapWith = enemyDropTableRows[rng.Next(enemyDropTableRows.Count)];
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

            //Swapping key items
            if (RandomiseKeyItems)
            {
                foreach (var item in keyItemLotRows)
                {
                    PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                    while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1 || IDBanks.forbiddenItemLotIds.Contains(rowToSwapWith.ID)) rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

                    SwapItemValues(item, rowToSwapWith, "lotItemId01");
                    SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
                    SwapItemValues(item, rowToSwapWith, "lotItemNum01");
                }
            }

            //Swapping shop inventory
            if (RandomiseShopInventory)
            {
                foreach (var item in shopItemRows)
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
                            PARAM.Row treasureItemToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                            while (Convert.ToInt64(treasureItemToSwapWith["lotItemCategory01"].Value) == -1 && !IDBanks.soulsIds.Contains(Convert.ToInt64(treasureItemToSwapWith["lotItemId01"].Value))) treasureItemToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

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
                            PARAM.Row rowToSwapWith = shopItemRows[rng.Next(shopItemRows.Count)];
                            while (rowToSwapWith.ID == item.ID) rowToSwapWith = shopItemRows[rng.Next(shopItemRows.Count)];

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

            //Swapping starting equipment
            if (RandomiseStartingEquipment)
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

            //Randomise Boss souls
            foreach (var item in bossItemsRows)
            {
                    //Find non-empty drop table slot
                    if (Convert.ToInt64(item["lotItemId01"].Value) != 0 && Convert.ToInt64(item["lotItemCategory01"].Value) != -1)
                    {
                        //Find a random item with a valid drop table slot
                        PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                        while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId01"].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1)
                        {
                            rowToSwapWith = enemyDropTableRows[rng.Next(enemyDropTableRows.Count)];
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
                    PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                    while (rowToSwapWith.ID == item.ID || Convert.ToInt64(rowToSwapWith["lotItemId01"].Value) == 0 || Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value) == -1)
                    {
                        rowToSwapWith = enemyDropTableRows[rng.Next(enemyDropTableRows.Count)];
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

            RepackGameBNDFile();
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

        private void InitRowLists()
        {
            enemyDropTableRows = new List<PARAM.Row>();
            treasureItemLotsRows = new List<PARAM.Row>();
            shopItemRows = new List<PARAM.Row>();
            keyItemLotRows = new List<PARAM.Row>();
            weaponDatabase = new List<PARAM.Row>();
            accesoryDatabase = new List<PARAM.Row>();
            armorDatabase = new List<PARAM.Row>();
            bossItemsRows = new List<PARAM.Row>();
            startingEquipmentRows = new List<PARAM.Row>();
            soulsDatabase = new List<PARAM.Row>();

            foreach (var item in parms["EquipParamWeapon"].Rows)
            {
                weaponDatabase.Add(item);
            }

            foreach (var item in parms["EquipParamProtector"].Rows)
            {
                armorDatabase.Add(item);
            }

            foreach (var item in parms["EquipParamAccessory"].Rows)
            {
                accesoryDatabase.Add(item);
            }

            foreach (var item in parms["ShopLineupParam"].Rows)
            {
                //Exclude spell vendors
                if (Convert.ToInt64(item["shopType"].Value) != 2 && Convert.ToInt64(item["shopType"].Value) != 3)
                {
                    shopItemRows.Add(item);
                }
            }

            foreach (var item in parms["ItemLotParam"].Rows)
            {
                GameWorld.ItemLots.Add(new ItemLot(item));

                //Nexus treasurelots
                if (IDBanks.treasureItemLotsNexus.Contains(item.ID))
                {
                    GameWorld.GetLevel(WorldID.NEXUS, 0).ItemLots.Add(new ItemLot(item));
                }

                //Boletaria treasureLots
                if (IDBanks.treasureItemLotsBoletarianPalace.Contains(item.ID))
                {
                    GameWorld.GetLevel(WorldID.BOLETARIA, 0).ItemLots.Add(new ItemLot(item));
                }

                if (IDBanks.treasureItemLots.Contains(item.ID))
                {
                    treasureItemLotsRows.Add(item);
                }

                if (IDBanks.enemyDropTableLotIds.Contains(item.ID))
                {
                    enemyDropTableRows.Add(item);
                }

                if (IDBanks.keyItems.Contains(item.ID))
                {
                    keyItemLotRows.Add(item);
                }

                if (IDBanks.bossItems.Contains(item.ID))
                {
                    bossItemsRows.Add(item);
                    soulsDatabase.Add(item);
                }

                if (IDBanks.soulsIds.Contains(item.ID))
                {
                    soulsDatabase.Add(item);
                }
            }
        }

        private bool VerifyItemLot(PARAM.Row item)
        {
            if(IDBanks.weaponIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
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

            if (consumeablesIds.Contains(Convert.ToInt64(item["lotItemId01"].Value)))
            {
                if (Convert.ToInt64(item["lotItemCategory01"].Value) != 1073741824) return false;
            }

            return true;
        }

        private void bttnRandomise_Click(object sender, RoutedEventArgs e)
        {
                RandomiseItems();
                MessageBox.Show("Randomisation Successful", "Items Randomised", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bttnInstructions_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory + @"\Readme.txt");
        }
    }
}
