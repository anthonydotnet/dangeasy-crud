using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Results;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{

    public class When_Getting : BaseTestFixture, IDisposable
    {

        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Profile _responseModel;

        public When_Getting()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);

            _responseModel = _model;
        }


        [Fact]
        public void All_List_Of_Documents_Are_Returned()
        {
            var responseList = new List<Profile>() { _responseModel }.AsQueryable();
            _repoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(responseList));

            var response = _service.GetAllAsync().Result as QueryResponse<Profile>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
        }


        [Fact]
        public void ById_Single_Document_Is_Returned()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            var response = _service.GetByIdAsync(_model.Id).Result as GetResponse<Profile>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
        }


        [Fact]
        public void ById_Invalid_Id_Causes_NotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(default(Profile)));

            var response = _service.GetByIdAsync(_model.Id).Result as GetNotFoundResponse<Profile>;

            Assert.Equal(StatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public void FirstOrDefault_Single_Document_Is_Returned()
        {
            const string sql = "SELECT * FROM Profile p WHERE p.FirstName = 'Anthony'";

            _repoMock.Setup(x => x.FirstOrDefaultAsync(sql)).Returns(Task.FromResult(_responseModel));

            var response = _service.FirstOrDefaultAsync(sql).Result as FirstOrDefaultResponse<Profile>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);            Assert.Equal(response.Data.Id, _model.Id);
        }
    }
}
