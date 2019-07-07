using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain.Layers;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain.Layers
{
    [TestClass]
    public class LayerGroupTest
    {
        [TestMethod]
        public void LayerGroupInitialisesCorrectValues()
        {
            //arrange
            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };

            // act
            var result = new LayerGroup(dto, Guid.NewGuid());

            //assert
            result.Name.ShouldBe(dto.Name);
            result.Type.ShouldBe(LayerType.Group);
            result.Layers.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task AddLayerAddsTheLayerToMap()
        {
            //arrange
            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };
            var layerGroup = new LayerGroup(dto, Guid.NewGuid());

            var newLayerDto = new LayerDto
            {
                Name = "testing",
                Type = LayerType.Default
            };

            // act
            var layer = await layerGroup.AddLayer(newLayerDto, Guid.NewGuid());

            //assert
            layerGroup.Layers.Count.ShouldBe(1);
            layerGroup.Layers.ShouldContain(layer);
        }
    }
}
