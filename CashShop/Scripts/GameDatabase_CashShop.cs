using Insthync.DevExtension;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class GameDatabase
	{
        public Gacha[] gachas;

		[DevExtMethods("LoadDataImplement")]
        protected async UniTask LoadDataImplement_CashShop(GameInstance gameInstance)
        {
            GameInstance.AddGachas(gachas);
		}

		[DevExtMethods("LoadReferredData")]
        public void LoadReferredData_CashShop()
        {
            GameInstance.AddGachas(gachas);

            List<Gacha> tempGachas = new List<Gacha>(GameInstance.Gachas.Values);
            gachas = tempGachas.ToArray();
		}
	}
}
