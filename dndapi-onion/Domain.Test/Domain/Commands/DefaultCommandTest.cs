﻿using Domain.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain.Commands
{
    [TestClass]
    public class DefaultCommandTest
    {
        [TestMethod]
        public void ConstructorSetsTheCommandResultTypeToDefault()
        {
            var result = new DefaultCommand();

            result.Id.ShouldNotBe(Guid.NewGuid());
            result.Type.ShouldBe(CommandType.Default);
        }
    }
}
