using Insthync.UnityEditorUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace MultiplayerARPG
{
    public abstract partial class BaseItem
    {
        [Category(100, "Cash Shop Generating Settings")]
        [SerializeField]
        protected CashShopItemGeneratingData[] cashShopItemGeneratingList = new CashShopItemGeneratingData[0];

        public void GenerateCashShopItems()
        {
            if (cashShopItemGeneratingList == null || cashShopItemGeneratingList.Length == 0)
                return;

            CashShopItemGeneratingData generatingData;
            CashShopItem cashShopItem;
            for (int i = 0; i < cashShopItemGeneratingList.Length; ++i)
            {
                generatingData = cashShopItemGeneratingList[i];
                cashShopItem = CreateInstance<CashShopItem>();
                cashShopItem.name = $"<CASHSHOPITEM_{name}_{i}>";
                cashShopItem.GenerateByItem(this, generatingData);
                GameInstance.CashShopItems[cashShopItem.DataId] = cashShopItem;
            }
        }
    }
}
