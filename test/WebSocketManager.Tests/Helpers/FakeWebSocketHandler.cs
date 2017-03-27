namespace WebSocketManager.Tests.Helpers
{
    internal class FakeWebSocketHandler : WebSocketHandler
    {
        public FakeWebSocketHandler(IWebSocketConnectionManager webSocketConnectionManager)
            : base(webSocketConnectionManager)
        {
        }
    }
}