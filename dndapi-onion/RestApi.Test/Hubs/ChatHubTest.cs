using AutoMapper;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Hubs;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Test.Hubs
{
    [TestClass]
    public class ChatHubTest
    {
        private ChatHub sut;

        private Mock<IChatService> chatService;
        private Mock<IJwtReader> jwtReader;
        private Mock<IMapper> mapper;

        [TestInitialize]
        public void Initialize()
        {
            chatService = new Mock<IChatService>();
            jwtReader = new Mock<IJwtReader>();
            mapper = new Mock<IMapper>();

            sut = new ChatHub(chatService.Object, jwtReader.Object, mapper.Object);
        }


    }
}
