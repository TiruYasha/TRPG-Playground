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
        const string CustomUsername = "username";

        private ChatMessage chatMessage;
        private User user;
        private Game game;

        [TestInitialize]
        public void Initialize()
        {
            user = new User()
            {
                UserName = "username"
            };
            game = new Game("", new Guid());
            chatMessage = new ChatMessage(Message, CustomUsername, user, game);
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
        public void ConstructorSetsTheCustomUsername()
        {
            chatMessage.CustomUsername.ShouldBe(CustomUsername);
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
        public void ConstructorSetsADefaultCustomUsernameIfEmpty()
        {
            chatMessage = new ChatMessage(Message, "", user, game);

            chatMessage.CustomUsername.ShouldBe(user.UserName);
        }

        [TestMethod]
        public void ConstructorExecutesCommand()
        {
            var message = "/r 1d1";
            var chatMessage = new ChatMessage(message, CustomUsername, user, game);

            (chatMessage.Command as NormalDiceRollCommand).RollResult.ShouldBe(1);
        }

    }
}
