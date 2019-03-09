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
    public class When_Deleting : BaseTestFixture, IDisposable
    {
        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Profile _responseModel;

        public When_Deleting()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);

            _responseModel = _model;
        }


        [Fact]
        public void Document_Is_Deleted()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            _repoMock.Setup(x => x.DeleteAsync(_model.Id)).Returns(Task.FromResult(true));

            var response = _service.DeleteAsync(_model.Id).Result;

            Assert.Equal(StatusCode.NoContent, response.StatusCode);
            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<object>()));
        }


        [Fact]
        public void Non_Existing_DocumentId_Causes_NotFound_Response()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(default(Profile)));

            var response = _service.DeleteAsync(_model.Id).Result;

            Assert.Equal(StatusCode.NotFound, response.StatusCode);

            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<object>()), Times.Never);
        }


        [Fact]
        public void Unknown_Error_Occured()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            _repoMock.Setup(x => x.DeleteAsync(_model.Id)).Returns(Task.FromResult(false));

            var response = _service.DeleteAsync(_model.Id).Result;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<object>()));
        }


        [Fact]
        public void Crash_Causes_Error_Response()
        {
            _responseModel = new Profile();
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            _repoMock.Setup(x => x.DeleteAsync(_model.Id)).ThrowsAsync(new Exception("Error message"));

            var response = _service.DeleteAsync(_model.Id).Result as BaseErrorResponse;

            Assert.ThrowsAsync<Exception>(() => _repoMock.Object.UpdateAsync(It.IsAny<Profile>()));

            Assert.Equal(StatusCode.Error, response.StatusCode);
        }
    }
}

