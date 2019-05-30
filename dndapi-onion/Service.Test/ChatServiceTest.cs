using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class ChatServiceTest
    {
        //private IChatService sut;

        //private Mock<IRepository> gameRepository;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    gameRepository = new Mock<IRepository>();

        //    sut = new ChatService(gameRepository.Object);
        //}

        //[TestMethod]
        //public async Task AddMessageToChatAsyncAddsTheMessageToTheChatOfTheGame()
        //{
        //    // Arrange
        //    var game = new Mock<Game>();
        //    var userId = new Guid();

        //    var chatMessage = new ChatMessage();
        //    var sendMessage = new SendMessageModel()
        //    {
        //        GameId = new Guid(),
        //        Message = "dsdsf",
        //        CustomUsername = "testing"
        //    };

        //    game.Setup(s => s.AddChatMessageAsync(sendMessage.Message, sendMessage.CustomUsername, userId)).ReturnsAsync(chatMessage);
        //    gameRepository.Setup(s => s.GetGameByIdAsync(sendMessage.GameId)).ReturnsAsync(game.Object);
        //    gameRepository.Setup(s => s.UpdateGameAsync(game.Object)).Returns(Task.CompletedTask).Verifiable();

        //    // Action
        //    var result = await sut.AddMessageToChatAsync(sendMessage, userId);

        //    // Assert
        //    gameRepository.VerifyAll();
        //    result.ShouldBe(chatMessage);
        //}
    }
}
