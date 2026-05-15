using Insthync.DevExtension;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class ResourcesFolderGameDatabase
	{
		[DevExtMethods("LoadDataImplement")]
        protected async UniTask LoadDataImplement_CashShop(GameInstance gameInstance)
		{
            Gacha[] gachas = Resources.LoadAll<Gacha>("");
            GameInstance.AddGachas(gachas);
		}
	}
}
