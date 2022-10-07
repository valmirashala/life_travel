using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelephant.Controllers;
using Travelephant.Data;
using Travelephant.Model;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Travelephant.Test.Controller
{
    public class BusInfoControllerTest
    {
        TravelephantContext _context;
        private BusInfoController repository;
        public static DbContextOptions<TravelephantContext> dbContextOptions { get; }
        public static string connectionString = "Server=localhost;Database=Travelephant.Data;Trusted_Connection=True";

        public BusInfoControllerTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TravelephantContext>();
            builder.UseSqlServer(connectionString)
            .UseInternalServiceProvider(serviceProvider);

            _context = new TravelephantContext(builder.Options);
            repository = new BusInfoController(_context);
        }

        [Fact]
        public void GetAll_Test()
        {
            var result = repository.GetAll();
            result.Should().HaveCount(67);
        }

        [Theory]
        [InlineData("Gjilan", "Prishtine", null, null, 1)]
        [InlineData("Prishtine", "Gjilan", null, null, 2)]
        [InlineData("Ferizaj", "Gjilan", null, null, 0)]
        [InlineData("Prizren", "Gjakove", 500, null, 1)]
        [InlineData("Rahovec", "Fushe Kosove", null, 900, 1)]
        public void Get_SpecificLine(string Departure, string Destination, int? starTime, int? endTime, int expected)
        {
            var result = repository.Get(Departure, Destination, starTime, endTime);
            result.Should().HaveCount(expected);
        }
        [Theory]
        [InlineData("bambi", "Sharr Travel", 755, 2)]
        public void SetPrice_TestTrue(string Username, string Name, int DepartureTime, double Price)
        {
            var result = repository.SetPrice(Username, Name, DepartureTime, Price).FirstOrDefault();
            Assert.Equal(Price, result.Price);
        }

        [Theory]
        [InlineData("ff", "Sharr Travel", 755, 5)]
        public void SetPrice_TestFalse(string Username, string Name, int DepartureTime, double Price)
        {
            var result = repository.SetPrice(Username, Name, DepartureTime, Price).FirstOrDefault();
            Assert.NotEqual(Price, result.Price);
        }
    }
}
