using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using WebSocketManager.Tests.Helpers;
using Xunit;

namespace WebSocketManager.Tests
{
    public class WebSocketHandlerTests
    {
        private readonly WebSocketHandler _handler;
        private readonly IWebSocketConnectionManager _manager;

        public WebSocketHandlerTests()
        {
            _manager = Substitute.For<IWebSocketConnectionManager>();
            _handler = new FakeWebSocketHandler(_manager);
        }

        public class OnConnected : WebSocketHandlerTests
        {
            [Fact]
            public async Task WhenNull_ShouldThrowArgumentNullException()
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.OnConnected(null));
            }

            [Fact]
            public async Task WhenSocketInstance_ShouldCallConnectionManager()
            {
                var socket = new FakeSocket();

                await _handler.OnConnected(socket);

                _manager.Received(1).AddSocket(socket);
            }

            [Fact]
            public async Task WhenClosedSocketInstance_ShouldNotSendConnectionEvent()
            {
                var socket = Substitute.For<WebSocket>();
                socket.State.Returns(WebSocketState.Closed);

                await _handler.OnConnected(socket);

                await socket.DidNotReceive()
                            .SendAsync(Arg.Any<ArraySegment<byte>>(),
                                       WebSocketMessageType.Text,
                                       true,
                                       CancellationToken.None);
            }

            [Fact]
            public async Task WhenOpenSocketInstance_ShouldSendConnectionEvent()
            {
                var socket = Substitute.For<WebSocket>();
                socket.State.Returns(WebSocketState.Open);

                await _handler.OnConnected(socket);

                await socket.Received(1)
                            .SendAsync(Arg.Any<ArraySegment<byte>>(),
                                       WebSocketMessageType.Text,
                                       true,
                                       CancellationToken.None);
            }
        }
    }
}