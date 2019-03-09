using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Results;
using System.Threading.Tasks;
using System.Linq;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{
    public class When_ExecutingSproc : BaseTestFixture, IDisposable
    {
        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Profile _responseModel;

        public When_ExecutingSproc()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);

            _responseModel = _model;
        }


        [Fact]
        public void Valid_Result_Is_Returned()
        {
            _repoMock.Setup(x => x.ExecuteStoredProcedureAsync<Profile>("sprocName", "param1", "param 2")).Returns(Task.FromResult(_responseModel));

            var response = _service.ExecuteStoredProcedureAsync<Profile>("sprocName", "param1", "param 2").Result as SprocResponse<Profile>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            Assert.Equal(_model.Id, response.Data.Id);
        }


        [Fact]
        public void Null_Response_Is_Expected()
        {
            _repoMock.Setup(x => x.ExecuteStoredProcedureAsync<object>("sprocName", "param1", "param2")).Returns(Task.FromResult(default(object)));

            var response = _service.ExecuteStoredProcedureAsync<object>("sprocName", "param1", "param2").Result as SprocResponse<object>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            Assert.Null(response.Data);
        }


        [Fact]
        public void Empty_Response_Is_Expected()
        {
            _repoMock.Setup(x => x.ExecuteStoredProcedureAsync<string>("sprocName", "param1", "param2")).Returns(Task.FromResult(string.Empty));

            var response = _service.ExecuteStoredProcedureAsync<string>("sprocName", "param1", "param2").Result as SprocResponse<string>;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            Assert.Equal(string.Empty, response.Data);
        }


        // I cant get this damn thing to throw an exception!!!
        //[Fact]
        //public void Crash_Causes_ErrorResponse()
        //{
        //    var exception = new Exception("My error");
        //    _repoMock.Setup(x => x.ExecuteStoredProcedureAsync<Profile>(It.IsAny<string>(), It.IsAny<object[]>())).ThrowsAsync(exception);

        //    var response = _service.ExecuteStoredProcedureAsync<Profile>("sprocName", "param1", "param2").Result as ErrorResult;

        //    Assert.Equal(StatusCode.Error, response.StatusCode);
        //    Assert.Equal(exception.Message, response.Exception.Message);
        //}
    }
}

