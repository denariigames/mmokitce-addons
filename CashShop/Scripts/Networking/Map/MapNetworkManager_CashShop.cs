using Insthync.DevExtension;

namespace MultiplayerARPG.MMO
{
    public partial class MapNetworkManager
	{
		[DevExtMethods("PrepareMapHandlers")]
        private void PrepareMapHandlers_CashShop()
		{
            // Server Message Handlers
            ServerCashShopMessageHandlers = gameObject.GetOrAddComponent<IServerCashShopMessageHandlers, MMOServerCashShopMessageHandlers>();
            ServerGachaMessageHandlers = gameObject.GetOrAddComponent<IServerGachaMessageHandlers, MMOServerGachaMessageHandlers>();
            // Client handlers
            ClientCashShopHandlers = gameObject.GetOrAddComponent<IClientCashShopHandlers, DefaultClientCashShopHandlers>();
            ClientGachaHandlers = gameObject.GetOrAddComponent<IClientGachaHandlers, DefaultClientGachaHandlers>();
		}
	}
}
