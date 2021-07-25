using System.Threading.Tasks;
using common.Responses;
using customer.write.Controllers;
using customer.write.Inputs;
using customer.write.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace test
{
    public class CustomerWriteTest
    {
        private readonly Mock<ICustomerRepository> repository = new();
        
        [Fact]
        public async Task AddCustomerAsync_ShouldReturnCreatedStatus()
        {
            // Arrange
            var input = new CustomerInput("John", "Doe");
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.AddCustomerAsync(input)).ReturnsAsync(new CustomerCreated());

            // Act
            var actual = await controller.AddCustomerAsync(input);

            // Assert
            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldPublishAddCustomerEvent()
        {
            // Arrange
            var input = new CustomerInput("John", "Doe");
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.AddCustomerAsync(input)).ReturnsAsync(new CustomerCreated());

            // Act
            var actual = await controller.AddCustomerAsync(input);

            // Assert
            repository.Verify(_ => _.AddCustomerAsync(input), Times.Once());
        }
    }
}