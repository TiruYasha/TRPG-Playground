using Domain.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
