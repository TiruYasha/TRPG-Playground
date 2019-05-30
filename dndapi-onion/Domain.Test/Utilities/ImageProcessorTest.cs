using System.IO;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Utilities
{
    [TestClass]
    public class ImageProcessorTest
    {
        private ImageProcesser sut;

        private string filePath = @"test/";

        [TestInitialize]
        public void Initialize()
        {
            sut = new ImageProcesser();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(filePath, true);
        }

        [TestMethod]
        public async Task UploadImageUploadsTheImageToTheSpecifiedPath()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";

            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, 2000, "Data", fileName);

            // Act
            await sut.SaveImage(file, filePath, fileName);

            // Assert
            var result = await File.ReadAllTextAsync(filePath + fileName);

            result.ShouldBe(content);
        }

        [TestMethod]
        public void UploadImageWhenEmptyThrowException()
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("")), 0, 0, "", "");

            // Act
            var exception = Should.Throw<FileException>(async () => await sut.SaveImage(file, filePath, ""));

            // Assert
            exception.Message.ShouldBe("The file may not be empty");
        }
    }
}
