using AutoMapper;
using Domain.Domain;
using Domain.Exceptions;
using Domain.RequestModels.Chat;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Hubs;
using RestApi.Models.Chat;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApi.Test.Hubs
{
    [TestClass]
    public class ChatHubTest
    {
        private ChatHub sut;

        private Mock<IChatService> chatService;
        private Mock<IJwtReader> jwtReader;
        private Mock<IMapper> mapper;
        private Mock<IHubCallerClients> mockClients;
        private Mock<IClientProxy> mockClientProxy;

       [TestInitialize]
        public void Initialize()
        {
            chatService = new Mock<IChatService>();
            jwtReader = new Mock<IJwtReader>();
            mapper = new Mock<IMapper>();

            mockClients = new Mock<IHubCallerClients>();
            mockClientProxy = new Mock<IClientProxy>();

            sut = new ChatHub(chatService.Object, jwtReader.Object, mapper.Object);
        }

        [TestMethod]
        public async Task SendToGroupAddsMessageToChatAndSendsTheMessageToTheWholeGroup()
        {
            // arrange
            var messageModel = new SendMessageModel()
            {
                CustomUsername = "hi",
                GameId = Guid.NewGuid(),
                Message = "Hello"
            };
            var userId = new Guid();
            var chatMessage = new ChatMessage();

            var receiveMessageModel = new ReceiveMessageModel();

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            chatService.Setup(s => s.AddMessageToChatAsync(messageModel, userId)).ReturnsAsync(chatMessage);
            mapper.Setup(s => s.Map<ChatMessage, ReceiveMessageModel>(chatMessage)).Returns(receiveMessageModel);

            mockClients.Setup(clients => clients.Group(messageModel.GameId.ToString())).Returns(mockClientProxy.Object);

            sut = new ChatHub(chatService.Object, jwtReader.Object, mapper.Object)
            {
                Clients = mockClients.Object
            };

            await sut.SendMessageToGroup(messageModel);

            mockClientProxy.Verify(s => s.SendCoreAsync("ReceiveMessage",
                It.Is<object[]>(o => o != null && o.Length == 1 && o[0] is ReceiveMessageModel),
                default(CancellationToken)));
        }

        [TestMethod]
        public async Task SendToGroupAddsMessageToChatSendRecieveMessageModelWithUnrecognizedCommandOnException()
        {
            // arrange
            var messageModel = new SendMessageModel()
            {
                CustomUsername = "hi",
                GameId = Guid.NewGuid(),
                Message = "Hello"
            };

            var userId = new Guid();
            var chatMessage = new ChatMessage();

            var receiveMessageModel = new ReceiveMessageModel();

            const string errorMessage = "The command does not exitt";

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            
            chatService.Setup(s => s.AddMessageToChatAsync(messageModel, userId)).ThrowsAsync(new UnrecognizedCommandException(errorMessage));

            mockClients.Setup(clients => clients.Caller).Returns(mockClientProxy.Object);

            sut = new ChatHub(chatService.Object, jwtReader.Object, mapper.Object)
            {
                Clients = mockClients.Object
            };

            await sut.SendMessageToGroup(messageModel);

            mockClientProxy.Verify(s => s.SendCoreAsync("ReceiveMessage",
                It.Is<object[]>(o => o != null && o.Length == 1 && o[0] is ReceiveMessageModel &&
                (o[0] as ReceiveMessageModel).CommandResult.Type == Domain.Domain.Commands.CommandType.UnrecognizedCommand &&
                (o[0] as ReceiveMessageModel).Message == errorMessage),
                default(CancellationToken)));
        }
    }
}
