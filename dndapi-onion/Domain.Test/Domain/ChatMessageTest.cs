using Domain.Domain;
using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test.Domain
{
    [TestClass]
    public class ChatMessageTest
    {
        const string Message = "Hello i am hello";

        private ChatMessage result;
        private User user;
        private Game game;
    

        [TestInitialize]
        public void Initialize()
        {
            user = new User();
            game = new Game();
            result = new ChatMessage(Message, user, game);
        }

        [TestMethod]
        public void ConstructorGeneratesId()
        {
            result.Id.ShouldNotBe(Guid.Empty);
        }

        [TestMethod]
        public void ConstructorSetsTheMessage()
        {
            result.Message.ShouldBe(Message);
        }

        [TestMethod]
        public void ConstructorSetTheCreatedDate()
        {
            result.CreatedDate.ShouldNotBe(DateTime.MinValue);
        }

        [TestMethod]
        public void ConstructorSetADefaultCommandResult()
        {
            result.CommandResult.ShouldNotBeNull();
            result.CommandResult.ShouldBeOfType<DefaultCommand>();
        }

        [TestMethod]
        public void ConstructorSetsTheUser()
        {
            result.User.ShouldBe(user);
        }

        [TestMethod]
        public void ConstructorSetsTheGame()
        {
            result.Game.ShouldBe(game);
        }
    }
}
