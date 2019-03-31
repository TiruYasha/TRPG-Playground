using Domain.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain
{
    [TestClass]
    public class GamePlayerTest
    {
        private GamePlayer sut;

        [TestMethod]
        public void NewGamePlayerInitializesUserAndGame()
        {
            // Arrange
            var user = new User();
            user.Id = new Guid();
            var game = new Game("test", user);

            // Action
            var result = new GamePlayer(game, user);

            // Assert
            result.User.ShouldBe(user);
            result.UserId.ShouldBe(user.Id);
            result.Game.ShouldBe(game);
            result.GameId.ShouldBe(game.Id);
        }

        [TestMethod]
        public void NewGamePlayerThrowsExceptionOnNullUser()
        {
            // Arrange
            User user = null;
            var game = new Game("test", new User());

            // Action
            var result = Should.Throw<ArgumentNullException>(() => new GamePlayer(game, user));

            // Assert
            result.Message.ShouldBe(@"Value cannot be null.
Parameter name: user");
        }

        [TestMethod]
        public void NewGamePlayerThrowsExceptionOnNullGame()
        {
            // Arrange
            User user = new User();
            Game game = null;

            // Action
            var result = Should.Throw<ArgumentNullException>(() => new GamePlayer(game, user));

            // Assert
            result.Message.ShouldBe(@"Value cannot be null.
Parameter name: game");
        }
    }
}
