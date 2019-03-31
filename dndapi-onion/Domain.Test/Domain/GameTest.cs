using Domain.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain
{
    [TestClass]
    public class GameTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void NewGameCreatesGameWithOwnerAndName()
        {
            // Arrange
            var name = "Elysia";
            var owner = new User();

            // Action
            var game = new Game(name, owner);

            // Assert
            game.Name.ShouldBe(name);
            game.Owner.ShouldBe(owner);
            game.Id.ToString().ShouldNotBe("00000000-0000-0000-0000-000000000000");
            
        }

        [TestMethod]
        public void NewGameThrowsExceptionOnEmptyName()
        {
            // Arrange
            var name = "";
            var owner = new User();

            // Action
            var result = Should.Throw<ArgumentException>(() => new Game(name, owner));

            // Assert
            result.Message.ShouldBe("The argument for parameter name was invalid");
        }

        [TestMethod]
        public void NewGameThrowsExceptionOnNullName()
        {
            // Arrange
            string name = null;
            var owner = new User();

            // Action
            var result = Should.Throw<ArgumentException>(() => new Game(name, owner));

            // Assert
            result.Message.ShouldBe("The argument for parameter name was invalid");
        }

        [TestMethod]
        public void NewGameThrowsExceptionOnNullOwner()
        {
            // Arrange
            string name = "test";
            User owner = null;
            
            // Action
            var result = Should.Throw<ArgumentException>(() => new Game(name, owner));

            // Assert
            result.Message.ShouldBe("The argument for parameter owner was invalid");
        }

        [TestMethod]
        public void JoinGameAddsAGamePlayer()
        {
            // Arrange
            var game = new Game("testing", new User());
            var user = new User
            {
                Id = new Guid("65a5b497-75b8-4729-9ca7-69152e319380")
            };

            // Action
            game.Join(user);

            // Assert
            game.Players.Count.ShouldBe(1);
        }

        [TestMethod]
        public void JoinGameThrowsExceptionWhenPlayerAlreadyExists()
        {
            // Arrange
            var game = new Game("testing", new User());
            var user = new User
            {
                Id = new Guid("65a5b497-75b8-4729-9ca7-69152e319380")
            };

            game.Join(user);

            // Action
            var ex = Should.Throw<ArgumentException>(() => game.Join(user));

            // Assert
            ex.Message.ShouldBe("The player cannot be added again");
        }

        [TestMethod]
        public void JoinGameThrowsExceptionWhenOwnerWantsToJoin()
        {
            // Arrange
            var owner = new User
            {
                Id = new Guid()
            };
            var game = new Game("testing", owner);
            
            // Action
            var ex = Should.Throw<ArgumentException>(() => game.Join(owner));

            // Assert
            ex.Message.ShouldBe("The player cannot be added again");
        }
    }
}
