namespace DemonSoulsItemRandomiser
{
    public enum ItemShopCategoryID
    {
        WEAPON,
        ARMOR,
        ACCESORY,
        CONSUMEABLE,
        SORCERY,
        MIRACLE
    }

    public static class EnumExtenstions
    {
        public static ItemCategoryID ShopCategoryToNormalCategory(this ItemShopCategoryID itemShopCategoryID)
        {
            switch (itemShopCategoryID)
            {
                case ItemShopCategoryID.WEAPON:
                    return ItemCategoryID.WEAPON;
                case ItemShopCategoryID.ARMOR:
                    return ItemCategoryID.ARMOR;
                case ItemShopCategoryID.ACCESORY:
                    return ItemCategoryID.ACCESORIES;
                case ItemShopCategoryID.CONSUMEABLE:
                    return ItemCategoryID.CONSUMABLES;
                default:
                    return ItemCategoryID.CONSUMABLES;
            }
        }
    }
}
