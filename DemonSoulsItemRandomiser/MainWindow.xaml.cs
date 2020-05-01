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

namespace DemonSoulsItemRandomiser
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string pathToParamDataFile = Directory.GetCurrentDirectory() + @"\Data\gameparamna.parambnd.dcx";
        string pathToParamDef = Directory.GetCurrentDirectory() + @"\Data\paramdef\paramdef.paramdefbnd.dcx";

        public event PropertyChangedEventHandler PropertyChanged;

        public bool RandomiseWorldTreasure { get; set; }
        public bool RandomiseKeyItems { get; set; }
        public bool RandomiseStartingEquipment { get; set; }
        public bool RandomiseEnemyDropTables { get; set; }
        public bool RandomiseShopInventory { get; set; }

        //ID Lists
        //IDS that we can't assign key items to incase we softlock the game
        List<long> forbiddenItemLotIds;

        //The orbs enemy drop after they die
        List<long> enemyDropTableLotIds;
        List<PARAM.Row> enemyDropTableRows;

        //World Drops (Glowing orbs on map)
        List<long> treasureItemLots;
        List<PARAM.Row> treasureItemLotsRows;

        //Items that allow progression
        List<long> keyItems;
        List<PARAM.Row> keyItemLotRows;

        //Items held in shops
        List<long> shopItems;
        List<PARAM.Row> shopItemRows;

        //Weapons
        List<long> weaponIds;
        List<PARAM.Row> weaponDatabase;

        //Weapons
        List<long> armorIds;
        List<PARAM.Row> armorDatabase;

        //Accesories
        List<long> accesoryIds;
        List<PARAM.Row> accesoryDatabase;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RandomiseWorldTreasure = true;
            InitIDLists();
        }

        private void RandomiseItems()
        {
            // Reading an original paramdefbnd
            var paramdefs = new Dictionary<string, PARAMDEF>();
            var paramdefbnd = BND3.Read(pathToParamDef);

            foreach (BinderFile file in paramdefbnd.Files)
            {
                var paramdef = PARAMDEF.Read(file.Bytes);
                paramdefs[paramdef.ParamType] = paramdef;
            }

            var parms = new Dictionary<string, PARAM>();
            var parambnd = BND3.Read(pathToParamDataFile);

            foreach (BinderFile file in parambnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                var param = PARAM.Read(file.Bytes);
                param.ApplyParamdef(paramdefs[param.ParamType]);
                parms[name] = param;
            }

            InitRowLists(parms["EquipParamWeapon"], parms["EquipParamProtector"], parms["EquipParamAccessory"], parms["ItemLotParam"], parms["ShopLineupParam"]);

            Random rng = new Random();

            //Swapping treasure
            if (RandomiseWorldTreasure)
            {
                foreach (var item in treasureItemLotsRows)
                {
                    PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                    if(rowToSwapWith.ID == item.ID) rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

                    SwapItemValues(item, rowToSwapWith, "lotItemId01");
                    SwapItemValues(item, rowToSwapWith, "lotItemCategory01");
                    SwapItemValues(item, rowToSwapWith, "lotItemNum01");
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
                            while (rowToSwapWith.ID == item.ID && Convert.ToInt64(rowToSwapWith["lotItemId0" + i].Value) != 0 && Convert.ToInt64(item["lotItemCategory0" + i].Value) != -1)
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

                }
            }

            //Swapping shop inventory
            if (RandomiseShopInventory)
            {
                foreach (var item in shopItemRows)
                {
                    //Decide if swapping with vendor or with world items
                    bool swapWithWorldItem = false;

                    //If item is not a consumeable
                    if (Convert.ToInt64(item["equipId"].Value) != 1073741824)
                    {
                        swapWithWorldItem = rng.Next(0, 2) > 0;

                        if (swapWithWorldItem)
                        {
                            PARAM.Row rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];
                            if (rowToSwapWith.ID == item.ID) rowToSwapWith = treasureItemLotsRows[rng.Next(treasureItemLotsRows.Count)];

                            var rowToSwapWithItemId = rowToSwapWith["lotItemId01"].Value;
                            var rowToSwapWithItemCategory = 0;

                            var currentRowItemId = item["equipId"].Value;
                            var currentRowItemCategory = 0;

                            switch (Convert.ToInt64(item["equipId"].Value))
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

                            switch (Convert.ToInt64(rowToSwapWith["lotItemCategory01"].Value))
                            {
                                case 0:
                                    rowToSwapWithItemCategory = 0;
                                    break;
                                case 268435456:
                                    rowToSwapWithItemCategory = 1;
                                    break;
                                case 536870912:
                                    rowToSwapWithItemCategory = 2;
                                    break;
                                case 1073741824:
                                    rowToSwapWithItemCategory = 3;
                                    break;
                            }

                            rowToSwapWith["lotItemId01"].Value = currentRowItemId;
                            rowToSwapWith["lotItemCategory01"].Value = currentRowItemCategory;

                            item["equipId"].Value = rowToSwapWithItemId;
                            item["equipType"].Value = rowToSwapWithItemCategory;
                        }

                        if (!swapWithWorldItem)
                        {
                            //Don't swap an item with itself
                            PARAM.Row rowToSwapWith = shopItemRows[rng.Next(shopItemRows.Count)];
                            while (rowToSwapWith.ID == item.ID) rowToSwapWith = shopItemRows[rng.Next(shopItemRows.Count)];

                            SwapItemValues(item, rowToSwapWith, "shopType");
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

            }

            foreach (BinderFile file in parambnd.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                file.Bytes = parms[name].Write();
            }
            parambnd.Write(pathToParamDataFile);
        }

        private void SwapItemValues(PARAM.Row val1, PARAM.Row val2, string valueName)
        {
            var original = val1[valueName].Value;
            var newVal = val2[valueName].Value;

            val1[valueName].Value = newVal;
            val2[valueName].Value = original;
        }

        private void InitIDLists()
        {
            forbiddenItemLotIds = new List<long>
            {
                10300,10301,10302,10303,10330,10331,10332,10360,10361,10362,10363,10364,10365,10366,10367,10505,10506,10507,10508,10509,10510,10511,10512,10513,10514,10515,10516,10517,10518,10520,10521,10522,10523,10524,10525,
                10526,10527,10528,10529,10536,10537,10538,10539,10541,10543,10544,10545,10546,10547,10548,10557,10558,10559,10560,10561,10562,10563,10564,10565,10566,10567,10568,10569,10582,10583,10584,10585,10586,10587,10589,
                10590,10591,10592,10595,10596,10597,16500,16501,16502,16503,16504,16505,16506,16507,16508,16509,16510,16511,16512,16513,16514,16515,16516,16517,16518,16519,16520,16521,16522,16523,16524,16525,16526,16527,16528,
                16529,16530,16531,16532,16533,16534,16535,16536,16537,16538,16539,16540,16541,16542,16543,16544,16545,16546,16547,16548,16549,16552,16554,16558,16559,16560,16561,16562,16563,16564,16566,16567,16568,16569,16570,
                16573,16574,16575,16576,16580,16581,16590
            };

            enemyDropTableLotIds = new List<long>
            {
                210000,210001,210005,210006,210010,210011,210020,210021,210030,210031,210040,210041,210045,210046,210050,210051,210060,210061,210065,210066,210070,210071,210075,210076,210080,210081,210085,210086,210090,
                210091,210092,220000,220001,220010,220011,220020,220021,220025,220026,220030,220031,220040,220045,220046,220050,220051,220055,220056,220060,220061,220065,220066,220070,220071,220080,220081,220090,220091,220100,
                220101,230000,230001,230010,230011,230020,230021,230025,230026,230030,230031,230040,230050,230051,230055,230056,230060,230061,230065,230066,230070,230071,230072,230080,230081,230090,230091,230095,230096,230100,
                230101,230110,230111,240000,240001,240010,240011,240020,240021,240025,240026,240030,240031,240100,240101,240110,240111,240040,240041,240045,240046,240050,240051,240055,240056,240060,240061,240070,240071,240080,
                240081,240082,240090,240091,310000,310001,310005,310006,310010,310011,310015,310016,310020,310021,310022,310025,310026,310027,310030,310050,310051,310060,310061,310062,310070,320000,320001,320010,320011,320020,
                320021,320022,320025,320026,320027,320030,320050,320051,320052,320055,320056,320057,320070,320071,320090,320091,320100,320101,320110,320120,320121,320122,320125,320126,320127,330000,330010,330011,400000,400001,
                400010,400011,400020,400021,400030,400031,400032,400033,400034,400040,400041,400050,400051,400060,400070,400071,400080,400090,410000,410001,410010,410011,410020,410030,410031,410040,410041,410050,410051,410060,
                410070,410071,410080,410090,420000,420001,420010,500000,500001,500002,500010,500011,500012,500020,500030,500040,500041,500050,500051,500052,500060,500061,500062,500070,500071,510000,510001,510002,510010,510011,
                510012,510020,510021,510022,510023,510030,510031,510032,510040,510041,510050,510051,510052,510053,510054,510060,510070,510071,510080,510081,510082,510090,510091,510092,510100,510101,510110,510111,510120,510121,
                510130,510131,520000,520001,520002,520010,520011,520012,520030,520031,520040,610000,610001,610002,610003,610010,610011,610012,610013,610020,610021,610022,610030,610031,610032,610040,610041,610050,610051,610052,
                610053,610054,610055,610056,610057,610060,610061,610062,610063,610070,610071,610072,610073,610080,610081,610082,610090,610100,610101,620000,620001,620002,620003,620004,620010,620011,620012,620013,620020,620021,
                620030,620031,620040,620041,620050,620051,620052,620053,620054,620060,620061,620062,620063,620070,620071,620072,620073,620080,620090,620100,620110,630050,630051,630052,630053,630054,810000,810001,810002,900000,
                900001,900002,900003,900004,900005,900006,900007,900008,900009,900010,900011,900012,900013,900014,900015,900016,900017,900018,900019,900020,900021,900022,900023,900024,900025,900026,900027,900028,900029,900030,
                900031,900032,900033,900034,900035,900036,900037,900038,900039,900040,900041,900042,900043,900044,900045,900046,900047
            };

            treasureItemLots = new List<long>
            {
                10120,10121,10122,10123,10124,10125,10126,10127,10128,10129,10130,10131,10132,10133,10134,10135,10136,10137,10138,10139,10140,10141,10142,10143,10144,10145,10146,10147,10148,10149,10150,10151,10152,10153,10154,
                10155,10156,10157,10158,10159,10160,10161,10162,10163,10164,10165,10166,10167,10168,10194,10201,10202,10203,10204,10205,10206,10207,10208,10209,10210,10211,10212,10213,10214,10215,10216,10217,10235,10236,10237,
                10238,10239,10240,10241,10242,10243,10244,10245,10246,10247,10248,10249,10260,10261,10262,10263,10264,10265,10266,10267,10268,10269,10282,10283,10284,10285,10286,10287,10288,10289,10290,10291,10292,10293,10294,
                10295,10296,10297,10298,10299,10400,10401,10402,10403,10405,10406,10407,10408,10409,10410,10411,10412,10413,10414,10419,10420,10421,10422,10423,10424,10425,10426,10427,10428,10429,10430,10431,10432,10433,10434,
                10435,10436,10437,10438,10454,10455,10460,10461,10462,10491,10505,10506,10507,10508,10509,10510,10511,10512,10513,10514,10515,10516,10517,10518,10520,10521,10522,10523,10524,10525,10526,10527,10528,10529,10536,
                10537,10538,10539,10541,10543,10544,10545,10546,10547,10548,10557,10558,10559,10560,10561,10562,10563,10564,10565,10566,10567,10568,10569,10582,10583,10584,10585,10586,10587,10589,10590,10591,10592,10595,10596,
                10597,10600,10601,10602,10603,10604,10605,10606,10607,10608,10609,10610,10611,10612,10613,10614,10615,10616,10617,10618,10619,10620,10621,10622,10623,10624,10625,10626,10628,10629,10630,10631,10632,10633,10634,
                10635,10636,10637,10638,10639,10640,10641,10642,10643,10644,10645,10646,10647,10648,10649,10661,10662,10663,10664,10665,10666,10667,10668,10669,10682,10683,10684,10685,10686,10687,10688,10689,10690,10691,10692,
                10693,10694,10695,10696,10697,10698,10740,10741,10750,10751,10760,10761,10810,10811,10812,10813,10814,10815,10816,10850,10860,10974,10975,10976,10977,11000,11001,15001,15002,15003,15004,15005,15006,15007,15008,
                15009,15010,15011,15012,15013,15014,15015,15016,15017,15018,15019,15020,15021,15022,15023,15024,15025,15026,15027,15028,15029,15030,15031,15032,15033,15034,15041,15042,15043,15044,15045,15046,15047,15048,15049,
                15050,15051,15052,15053,15054,15055,15057,15058,15059,15060,15061,15063,15064,15065,15066,15067,15068,15081,15082,15083,15087,15088,15089,15092,15093,15094,15095,15096,15097,15098,15402,15403,15500,15501,15502,
                15503,15527,15528,15529,15530,15537,15538,15539,15600,15601,15700,15701,16200,16201,16202,16203,16204,16205,16218,16230,16231,16232,16233,16234,16235,16236,16237,16238,16239,16240,16241,16242,16243,16290,16400,
                16401,16402,16403,16404,16405,16406,16407,16408,16409,16410,16411,16412,16413,16414,16415,16416,16417,16418,16419,16420,16421,16422,16423,16424,16425,16426,16427,16428,16429,16430,16431,16432,16433,16434,16435,
                16436,16437,16438,16439,16440,16441,16442,16443,16444,16445,16446,16462,16463,16470,16471,16472,16473,16474,16475,16500,16501,16502,16503,16504,16505,16506,16507,16508,16509,16510,16511,16512,16513,16514,16515,
                16516,16517,16518,16519,16520,16521,16522,16523,16524,16525,16526,16527,16528,16529,16530,16531,16532,16533,16534,16535,16536,16537,16538,16539,16540,16541,16542,16543,16544,16545,16546,16547,16548,16549,16552,
                16554,16558,16559,16560,16561,16562,16563,16564,16566,16567,16568,16569,16570,16573,16574,16575,16576,16580,16581,16590,16601,16614,16616,16617,16640,16650,16651,16652,16653,16654,16655,16656,16657,16658,16659,
                16660,16661,16662,16663,16664,16665,16666,16667,16668,16669,16670,16671,16672,10731,10191,10192,10193,10710,10711,10712,10713,10714,10716

            };

            keyItems = new List<long>
            {
                10200,10106,10107,10500,10501,10502,10503,10504,10594,10553,10554,10555,10556
            };

            shopItems = new List<long>
            {
                1000,1001,1002,1003,1004,1005,1006,1007,1008,1009,1010,1011,1012,1013,1014,1015,1050,1051,1052,1053,1054,1100,1101,1102,1103,1104,1105,1150,1151,1152,1153,1154,2000,2001,2002,2003,2004,2005,2006,2007,2008,2009,
                2010,2011,2012,2013,2014,2015,2016,2050,2051,2052,2053,2054,3000,3001,3002,3003,3004,3005,3006,3007,3008,3009,3010,3014,4000,4001,4002,4003,4004,4005,4006,4007,4008,4009,4010,4011,4012,4013,4014,4050,4051,4052,
                4053,4054,4055,5000,5001,5002,5003,5004,5050,5051,5052,5053,5054,6000,6001,6002,6003,6004,6005,6006,6007,6050,6051,6100,6101,6151,6153,7001,7002,7003,7004,7005,7006,7007,7050,7051,7052,7053,7054,7055,7056,7057,
                7058,7059,7060,7061,7062,7063,7064,7065,7100,7101,7102,7103,7104,7105,8000,8001,8002,8003,8050,8051,8052,8053,8054,8055,8056,8057,8058,8059,8060,8061,9000,9001,9002,9003,9004,9005,9006,9007,9008
            };
        }

        private void InitRowLists(PARAM weapons, PARAM armor, PARAM accesories, PARAM itemLots, PARAM shopItems)
        {
            enemyDropTableRows = new List<PARAM.Row>();
            treasureItemLotsRows = new List<PARAM.Row>();
            shopItemRows = new List<PARAM.Row>();
            keyItemLotRows = new List<PARAM.Row>();
            weaponDatabase = new List<PARAM.Row>();
            accesoryDatabase = new List<PARAM.Row>();
            armorDatabase = new List<PARAM.Row>();

            foreach (var item in weapons.Rows)
            {
                weaponDatabase.Add(item);
            }

            foreach (var item in armor.Rows)
            {
                armorDatabase.Add(item);
            }

            foreach (var item in accesories.Rows)
            {
                accesoryDatabase.Add(item);
            }

            foreach (var item in shopItems.Rows)
            {
                //Exclude spell vendors
                if (Convert.ToInt64(item["shopType"].Value) != 3 || Convert.ToInt64(item["shopType"].Value) != 4)
                {
                    shopItemRows.Add(item);
                }
            }

            foreach (var item in itemLots.Rows)
            {
                if(keyItems.Contains(item.ID))
                {
                    keyItemLotRows.Add(item);
                }
                else
                {
                    if (treasureItemLots.Contains(item.ID))
                    {
                        treasureItemLotsRows.Add(item);
                    }
                    else if (enemyDropTableLotIds.Contains(item.ID))
                    {
                        enemyDropTableRows.Add(item);
                    }
                    else if (keyItems.Contains(item.ID))
                    {
                        keyItemLotRows.Add(item);
                    }
                }
            }
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
