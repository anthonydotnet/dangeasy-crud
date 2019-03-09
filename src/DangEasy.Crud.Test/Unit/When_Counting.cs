using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Results;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{
    public class When_Counting : BaseTestFixture, IDisposable
    {

        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;

        public When_Counting()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }


        [Fact]
        public void Count_Has_Result()
        {
            _repoMock.Setup(x => x.CountAsync()).Returns(Task.FromResult(1));

            var response = _service.CountAsync().Result as CountResponse;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            Assert.Equal(1, response.Count);
        }


        [Fact]
        public void Count_By_Sql_Has_Result()
        {
            const string sql = "SELECT * FROM Profile p WHERE p.FirstName = 'Anthony'";

            _repoMock.Setup(x => x.CountAsync(sql)).Returns(Task.FromResult(1));

            var response = _service.CountAsync(sql).Result as CountResponse;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            Assert.Equal(1, response.Count);
        }
    }
}
