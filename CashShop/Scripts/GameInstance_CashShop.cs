using Insthync.DevExtension;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if ENABLE_PURCHASING && (UNITY_IOS || UNITY_ANDROID)
using UnityEngine.Purchasing;
#endif

namespace MultiplayerARPG
{
    public partial class GameInstance
	{
        public static IClientCashShopHandlers ClientCashShopHandlers { get; set; }
        public static IClientGachaHandlers ClientGachaHandlers { get; set; }
        public static readonly Dictionary<int, Gacha> Gachas = new Dictionary<int, Gacha>();

		[DevExtMethods("ClearData")]
        public static void ClearData_CashShop()
		{
            Gachas.Clear();
		}

		[DevExtMethods("LoadedGameData")]
        public void LoadedGameData_CashShop()
        {
            if (Application.isPlaying)
                InitializePurchasing();
		}

        public static void AddGachas(params Gacha[] gachas)
        {
            AddGachas((IEnumerable<Gacha>)gachas);
        }

        public static void AddGachas(IEnumerable<Gacha> gachas)
        {
            AddManyGameData(Gachas, gachas);
        }
	}
}
