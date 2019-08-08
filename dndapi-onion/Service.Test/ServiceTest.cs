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
        protected IMapper Mapper;

        protected TServiceInterface Sut;
        protected DndContext Context;

        protected GameDataBuilder GameDataBuilder;

        public virtual void Initialize()
        {
            IConfigurationProvider config = new MapperConfiguration(d => d.AddProfile<MyProfile>());
            Mapper = new Mapper(config);

            var options = new DbContextOptionsBuilder<DndContext>()
                .UseInMemoryDatabase("inmemorydbdnd")
                .Options;
            Context = new DndContext(options);
            GameDataBuilder = new GameDataBuilder();
        }

        public virtual void Cleanup()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
