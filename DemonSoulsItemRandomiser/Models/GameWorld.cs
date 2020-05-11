using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class GameWorld
    {
        //Reference
        public Dictionary<string, PARAM> Parms
        {
            get
            {
                return MainWindow.parms;
            }
        }

        //All Itemlots
        public List<Level> GameLevels { get; set; }
        public List<ItemLot> ItemLots { get; set; }

        public List<NPC> NPCs { get; set; }
        public List<Enemy> EnemyItemLots { get; set; }
        public List<NPCShopkeeper> ShopKeepers { get; set; }

        public List<PARAM.Row> enemyDropTableRows;

        //World Drops (Glowing orbs on map)
        public List<PARAM.Row> treasureItemLotsRows;

        //Items that allow progression
        public List<PARAM.Row> keyItemLotRows;

        //Items held in shops
        public List<PARAM.Row> shopItemRows;

        //Boss items
        public List<PARAM.Row> bossItemsRows;

        //Starting Characters
        public List<PARAM.Row> startingEquipmentRows;

        //Weapons
        public List<PARAM.Row> weaponDatabase;

        //Weapons
        public List<PARAM.Row> armorDatabase;

        //Accesories
        public List<PARAM.Row> accesoryDatabase;

        //Souls items
        public List<PARAM.Row> soulsDatabase;

        //Consumeables
        public List<long> consumeablesIds;

        public GameWorld()
        {
            ItemLots = new List<ItemLot>();

            //Create level objects
            GameLevels = new List<Level>
            {
                new Level(WorldID.TUTORIAL, 0, "Tutorial Area"),
                new Level(WorldID.TUTORIAL, 1, "Tutorial Area"),
                new Level(WorldID.TUTORIAL, 2, "Tutorial Area"),
                new Level(WorldID.NEXUS, 0, "The Nexus"),
                new Level(WorldID.NEXUS, 1, "Endgame"),
                new Level(WorldID.BOLETARIA, 0, "Boletarian Palace"),
                new Level(WorldID.BOLETARIA, 1, "Phalanx Archstone"),
                new Level(WorldID.BOLETARIA, 2, "Tower Knight Archstone"),
                new Level(WorldID.BOLETARIA, 3, "Penetrator Archstone"),
                new Level(WorldID.STONEFANG_TUNNEL, 0, "Stonefang Tunnel"),
                new Level(WorldID.STONEFANG_TUNNEL, 1, "Armor Spider Archstone"),
                new Level(WorldID.STONEFANG_TUNNEL, 2, "Flamelurker Archstone"),
                new Level(WorldID.TOWER_OF_LATRIA, 0, "Tower of Latria"),
                new Level(WorldID.TOWER_OF_LATRIA, 1, "Fools Idol Archstone"),
                new Level(WorldID.TOWER_OF_LATRIA, 2, "Maneater Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 0, "Shrine of Storms"),
                new Level(WorldID.SHRINE_OF_STORMS, 1, "Adjudicator Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 2, "Old Hero Archstone"),
                new Level(WorldID.VALLEY_OF_DEFILEMENT, 0, "Valley of Defilement"),
                new Level(WorldID.SHRINE_OF_STORMS, 1, "Leechmonger Archstone"),
                new Level(WorldID.SHRINE_OF_STORMS, 2, "Dirty Colossus Archstone")
            };

            ShopKeepers = new List<NPCShopkeeper>
            {
                new NPCShopkeeper("Boldwin", GetLevel(WorldID.NEXUS, 0)),
                new NPCShopkeeper("Dregling", GetLevel(WorldID.BOLETARIA, 0)),
                new NPCShopkeeper("Filthy Man", GetLevel(WorldID.STONEFANG_TUNNEL, 0)),
                new NPCShopkeeper("Mistress", GetLevel(WorldID.TOWER_OF_LATRIA, 0)),
                new NPCShopkeeper("Blige", GetLevel(WorldID.SHRINE_OF_STORMS, 0)),
                new NPCShopkeeper("Filthy Woman", GetLevel(WorldID.VALLEY_OF_DEFILEMENT, 0)),
                new NPCShopkeeper("Patches", GetLevel(WorldID.NEXUS, 0))
            };

            NPCs = new List<NPC>()
            {
                
            };

            //Populate Param row lists with items from bnd file
            InitRowLists();
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

            EnemyItemLots = new List<Enemy>();

            foreach (var item in Parms["EquipParamWeapon"].Rows)
            {
                weaponDatabase.Add(item);
            }

            foreach (var item in Parms["EquipParamProtector"].Rows)
            {
                armorDatabase.Add(item);
            }

            foreach (var item in Parms["EquipParamAccessory"].Rows)
            {
                accesoryDatabase.Add(item);
            }

            foreach (var item in Parms["ShopLineupParam"].Rows)
            {
                //Exclude spell vendors
                if (Convert.ToInt64(item["shopType"].Value) != 2 && Convert.ToInt64(item["shopType"].Value) != 3)
                {
                    shopItemRows.Add(item);

                    if (IDBanks.boldwinShopItemIds.Contains(item.ID)) GetShopKeeper("Boldwin").Items.Add(new ItemShop(item, GetShopKeeper("Boldwin")));
                    if (IDBanks.dreglingShopItemIds.Contains(item.ID)) GetShopKeeper("Dregling").Items.Add(new ItemShop(item, GetShopKeeper("Dregling")));
                    if (IDBanks.filthyManShopItemIds.Contains(item.ID)) GetShopKeeper("Filthy Man").Items.Add(new ItemShop(item, GetShopKeeper("Filthy Man")));
                    if (IDBanks.mistressShopItemIds.Contains(item.ID)) GetShopKeeper("Mistress").Items.Add(new ItemShop(item, GetShopKeeper("Mistress")));
                    if (IDBanks.BligeShopItemIds.Contains(item.ID)) GetShopKeeper("Blige").Items.Add(new ItemShop(item, GetShopKeeper("Blige")));
                    if (IDBanks.filthyWomanShopItemIds.Contains(item.ID)) GetShopKeeper("Filthy Woman").Items.Add(new ItemShop(item, GetShopKeeper("Filthy Woman")));
                    if (IDBanks.patchesShopItemIds.Contains(item.ID)) GetShopKeeper("Patches").Items.Add(new ItemShop(item, GetShopKeeper("Patches")));
                }
            }

            foreach (var item in Parms["ItemLotParam"].Rows)
            {
                ItemLots.Add(new ItemLot(item));

                //Nexus treasurelots
                if (IDBanks.treasureItemLotsNexus.Contains(item.ID))
                {
                    GetLevel(WorldID.NEXUS, 0).ItemLots.Add(new ItemLot(item));
                }

                //Boletaria treasureLots
                if (IDBanks.treasureItemLotsBoletarianPalace.Contains(item.ID))
                {
                    GetLevel(WorldID.BOLETARIA, 0).ItemLots.Add(new ItemLot(item));
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

        public void SaveChangesToData()
        {
            //Save itemlots
            foreach (var item in ItemLots)
            {
                item.Save();
            }

            //Save shop items
            foreach (var shopKeeper in ShopKeepers)
            {
                foreach (var shopItem in shopKeeper.Items)
                {
                    shopItem.Save();
                }
            }
        }

        public List<ItemLot> GetAllTreasureLots()
        {
            List<ItemLot> returnItems = new List<ItemLot>();

            foreach (var item in ItemLots)
            {
                if (IDBanks.treasureItemLots.Contains(item.ID)) returnItems.Add(item);
            }

            return returnItems;
        }

        public Level GetLevel(WorldID worldId, int level)
        {
            Level returnLevel = null;

            foreach (var item in GameLevels)
            {
                if (item.World == worldId && item.LevelID == level)
                {
                    returnLevel = item;
                    break;
                }
            }

            return returnLevel;
        }

        public NPCShopkeeper GetShopKeeper(string name)
        {
            NPCShopkeeper returnShopKeeper = null;

            foreach (var item in ShopKeepers)
            {
                if (item.Name.ToLower().Trim() == name.ToLower().Trim())
                {
                    returnShopKeeper = item;
                    break;
                }
            }

            return returnShopKeeper;
        }

        public List<ItemShop> GetAllShopItems()
        {
            List<ItemShop> returnItems = new List<ItemShop>();

            foreach (var shopKeeper in ShopKeepers)
            {
                foreach (var shopItem in shopKeeper.Items)
                {
                    returnItems.Add(shopItem);
                }
            }

            return returnItems;
        }

    }
}
