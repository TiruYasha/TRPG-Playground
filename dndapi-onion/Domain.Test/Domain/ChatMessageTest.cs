using Domain.Domain;
using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain
{
    [TestClass]
    public class ChatMessageTest
    {
        const string Message = "Hello i am hello";

        private ChatMessage chatMessage;
        private User user;
        private Game game;
    

        [TestInitialize]
        public void Initialize()
        {
            user = new User();
            game = new Game();
            chatMessage = new ChatMessage(Message, user, game);
        }

        [TestMethod]
        public void ConstructorGeneratesId()
        {
            chatMessage.Id.ShouldNotBe(Guid.Empty);
        }

        [TestMethod]
        public void ConstructorSetsTheMessage()
        {
            chatMessage.Message.ShouldBe(Message);
        }

        [TestMethod]
        public void ConstructorSetTheCreatedDate()
        {
            chatMessage.CreatedDate.ShouldNotBe(DateTime.MinValue);
        }
      
        [TestMethod]
        public void ConstructorSetsTheUser()
        {
            chatMessage.User.ShouldBe(user);
        }

        [TestMethod]
        public void ConstructorSetsTheGame()
        {
            chatMessage.Game.ShouldBe(game);
        }

        [TestMethod]
        public void ConstructorSetsADefaultCommandResult()
        {
            chatMessage.Command.ShouldNotBeNull();
            chatMessage.Command.ShouldBeOfType<DefaultCommand>();
        }

        [TestMethod]
        public void ConstructorExecutesCommand()
        {
            var message = "/r 1d1";
            var chatMessage = new ChatMessage(message, user, game);

            (chatMessage.Command as NormalDiceRollCommand).RollResult.ShouldBe(1);
        }

    }
}
