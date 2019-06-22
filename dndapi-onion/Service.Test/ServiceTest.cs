using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DataAccess;
using Domain.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Service.Test
{
    public abstract class ServiceTest<TServiceInterface>
    {
        protected IMapper mapper;

        protected TServiceInterface sut;
        protected DndContext context;

        protected GameDataBuilder gameDataBuilder;

        public virtual void Initialize()
        {
            IConfigurationProvider config = new MapperConfiguration(d => d.AddProfile<MyProfile>());
            mapper = new Mapper(config);

            var options = new DbContextOptionsBuilder<DndContext>()
                .UseInMemoryDatabase("inmemorydbdnd")
                .Options;
            context = new DndContext(options);
            gameDataBuilder = new GameDataBuilder();
        }
    }
}
