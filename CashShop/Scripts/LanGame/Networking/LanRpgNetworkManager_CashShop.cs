using Insthync.DevExtension;

namespace MultiplayerARPG
{
    public partial class LanRpgNetworkManager
    {
		[DevExtMethods("PrepareLanRpgHandlers")]
        private void PrepareLanRpgHandlers_CashShop()
        {
            // Server Message Handlers
            ServerCashShopMessageHandlers = gameObject.GetOrAddComponent<IServerCashShopMessageHandlers, LanRpgServerCashShopMessageHandlers>();
            ServerGachaMessageHandlers = gameObject.GetOrAddComponent<IServerGachaMessageHandlers, LanRpgServerGachaMessageHandlers>();
            // Client handlers
            ClientCashShopHandlers = gameObject.GetOrAddComponent<IClientCashShopHandlers, DefaultClientCashShopHandlers>();
            ClientGachaHandlers = gameObject.GetOrAddComponent<IClientGachaHandlers, DefaultClientGachaHandlers>();
		}
    }
}
