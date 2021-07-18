using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using customer.read.Controllers;
using customer.read.Interfaces;
using Microsoft.AspNetCore.Mvc;
using customer.read.Payloads;
using Moq;
using Xunit;
using System.Linq;
using common.Responses;

namespace test.Controllers
{
    public class CustomerControllerTest
    {
        
        private readonly Mock<ICustomerRepository> repository = new();

        [Fact]
        public async Task GetCustomersAsync_ReturnsCustomerList()
        {
            // Arrange
            const int expected = 1;
            var controller = new CustomerController(repository.Object);
            var customers = new List<CustomerPayload>
            {
                new CustomerPayload
                {
                    CustomerId = Guid.NewGuid(),
                    Name = "Mike",
                    Surname = "Mock"
                }
            };
            repository.Setup(_ => _.GetCustomersAsync()).ReturnsAsync(customers);
            
            // Act
            var result = await controller.GetCustomersAsync();
            var value = (result.Result as OkObjectResult).Value as IEnumerable<CustomerPayload>;
            var actual = value.ToList().Count;

            // Assert
            Assert.True(actual == expected);
        }

        [Fact]
        public async Task GetCustomersAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.GetCustomersAsync()).ReturnsAsync(new List<CustomerPayload>());

            // Act
            var actual = await controller.GetCustomersAsync();

            // Assert
            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Fact]
        public async Task GetCustomerAsync_ReturnsCustomer()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new CustomerPayload
            {
                CustomerId = id,
                Name = "Mike",
                Surname = "Mock"
            };
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.GetCustomerAsync(id)).ReturnsAsync(expected);

            // Act
            var result = await controller.GetCustomerAsync(id);
            var actual = (result.Result as OkObjectResult).Value as CustomerPayload;

            // Assert
            Assert.True(actual.Equals(expected));
        }

        [Fact]
        public async Task GetCustomerAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new CustomerPayload
            {
                CustomerId = id,
                Name = "Mike",
                Surname = "Mock"
            };
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.GetCustomerAsync(id)).ReturnsAsync(expected);

            // Act
            var actual = (await controller.GetCustomerAsync(id)).Result;

            // Assert
           Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async Task GetCustomerAsync_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var controller = new CustomerController(repository.Object);
            repository.Setup(_ => _.GetCustomerAsync(It.IsAny<Guid>())).ReturnsAsync(new CustomerInvalidId());

            // Act
            var actual = (await controller.GetCustomerAsync(It.IsAny<Guid>())).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(actual);
        }
    }
}