using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace DemonSoulsItemRandomiser.Util
{
    public static class ItemDescriptionTranslator
    {
        public static string TranslateItem(ItemCategoryID itemCategory, long itemID)
        {
            string pathToDatabase = Directory.GetCurrentDirectory() + @"\Data\DeS.db";
            string translatedDescription = "";

            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=" + pathToDatabase + ";Version=3;");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connection to database for translation: " + ex.Message);
            }

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            string tableName;

            switch (itemCategory)
            {
                case ItemCategoryID.WEAPON:
                    tableName = "EquipParamWeapon";
                    break;
                case ItemCategoryID.ARMOR:
                    tableName = "EquipParamProtector";
                    break;
                case ItemCategoryID.ACCESORIES:
                    tableName = "EquipParamAccessory";
                    break;
                case ItemCategoryID.CONSUMABLES:
                    tableName = "EquipParamGoods";
                    break;
                default:
                    tableName = "EquipParamGoods";
                    break;
            }

            sqlite_cmd.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", "description", tableName, "id", itemID);

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                translatedDescription = sqlite_datareader.GetString(0);
            }

            sqlite_conn.Close();

            return translatedDescription;
        }

        public static string TranslateItem(ItemShopCategoryID shopItemCategory, long itemID)
        {
            return TranslateItem(shopItemCategory.ShopCategoryToNormalCategory(), itemID);
        }
    }
}
