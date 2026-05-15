using Insthync.DevExtension;
using LiteNetLibManager;

namespace MultiplayerARPG
{
    public partial class BaseGameNetworkManager
	{
        // Server Message Handlers
        protected IServerCashShopMessageHandlers ServerCashShopMessageHandlers { get; set; }
        protected IServerGachaMessageHandlers ServerGachaMessageHandlers { get; set; }
        // Client handlers
        protected IClientCashShopHandlers ClientCashShopHandlers { get; set; }
        protected IClientGachaHandlers ClientGachaHandlers { get; set; }

		[DevExtMethods("Clean")]
        protected virtual void Clean_CashShop()
		{
            ClientCashShopActions.Clean();
            ClientGachaActions.Clean();
		}

		[DevExtMethods("RegisterHandlerMessages")]
        protected virtual void RegisterHandlerMessages_CashShop()
		{
            // Cash shop
            if (ServerCashShopMessageHandlers != null)
            {
                RegisterRequestToServer<EmptyMessage, ResponseCashShopInfoMessage>(GameNetworkingConsts.CashShopInfo, ServerCashShopMessageHandlers.HandleRequestCashShopInfo);
                RegisterRequestToServer<EmptyMessage, ResponseCashPackageInfoMessage>(GameNetworkingConsts.CashPackageInfo, ServerCashShopMessageHandlers.HandleRequestCashPackageInfo);
                RegisterRequestToServer<RequestCashShopBuyMessage, ResponseCashShopBuyMessage>(GameNetworkingConsts.CashShopBuy, ServerCashShopMessageHandlers.HandleRequestCashShopBuy);
                RegisterRequestToServer<RequestCashPackageBuyValidationMessage, ResponseCashPackageBuyValidationMessage>(GameNetworkingConsts.CashPackageBuyValidation, ServerCashShopMessageHandlers.HandleRequestCashPackageBuyValidation);
            }
            // Gacha
            if (ServerGachaMessageHandlers != null)
            {
                RegisterRequestToServer<EmptyMessage, ResponseGachaInfoMessage>(GameNetworkingConsts.GachaInfo, ServerGachaMessageHandlers.HandleRequestGachaInfo);
                RegisterRequestToServer<RequestOpenGachaMessage, ResponseOpenGachaMessage>(GameNetworkingConsts.OpenGacha, ServerGachaMessageHandlers.HandleRequestOpenGacha);
            }
		}

		[DevExtMethods("SetClientHandlersRef")]
        protected virtual void SetClientHandlersRef_CashShop()
		{
            GameInstance.ClientCashShopHandlers = ClientCashShopHandlers;
            GameInstance.ClientGachaHandlers = ClientGachaHandlers;
		}
	}
}
