using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Results;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{

    public class When_Getting_Fails : BaseTestFixture, IDisposable
    {

        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Exception _exception;
        public When_Getting_Fails() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);

            _exception = new Exception("My error");
        }


        [Fact]
        public void All_List_Of_Documents_Causes_ErrorResponse()
        {
            _repoMock.Setup(x => x.GetAllAsync()).ThrowsAsync(_exception);

            var response = _service.GetAllAsync().Result as BaseErrorResponse;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.Equal(_exception.Message, response.Exception.Message);
        }


        [Fact]
        public void ById_Single_Document_Causes_ErrorResponse()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).ThrowsAsync(_exception);

            var response = _service.GetByIdAsync(_model.Id).Result as BaseErrorResponse;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.Equal(_exception.Message, response.Exception.Message);
        }





        [Fact]
        public void Count_Causes_ErrorResponse()
        {
            _repoMock.Setup(x => x.CountAsync()).ThrowsAsync(_exception);

            var response = _service.CountAsync().Result as BaseErrorResponse;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.Equal(_exception.Message, response.Exception.Message);
        }


        [Fact]
        public void Count_By_Sql_Causes_ErrorResponse()
        {
            const string sql = "SELECT * FROM Profile p WHERE p.FirstName = 'Anthony'";

            _repoMock.Setup(x => x.CountAsync(sql)).ThrowsAsync(_exception);

            var response = _service.CountAsync(sql).Result as BaseErrorResponse;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.Equal(_exception.Message, response.Exception.Message);
        }
    }
}
