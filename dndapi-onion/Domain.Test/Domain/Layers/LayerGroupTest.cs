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
            var name = "test";

            // act
            var result = new LayerGroup(name, Guid.NewGuid());

            //assert
            result.Name.ShouldBe(name);
            result.Type.ShouldBe(LayerType.Group);
            result.Layers.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task AddLayerAddsTheLayerToMap()
        {
            //arrange
            var name = "test";
            var layerGroup = new LayerGroup(name, Guid.NewGuid());

            var dto = new LayerDto
            {
                Name = "testing",
                Type = LayerType.Default
            };

            // act
            var layer = await layerGroup.AddLayer(dto, Guid.NewGuid());

            //assert
            layerGroup.Layers.Count.ShouldBe(1);
            layerGroup.Layers.ShouldContain(layer);
        }
    }
}
