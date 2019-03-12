using Xunit;
using System;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using System.Threading.Tasks;
using DangEasy.Crud.Enums;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{
    public class When_Updating : BaseTestFixture, IDisposable
    {
        Profile _modelToSave;
        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Profile _responseModel;

        public When_Updating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);

            _modelToSave = new Profile
            {
                Id = "Model.1",
                FirstName = "Boo",
                LastName = "Dang",
                Updated = new DateTime(2022, 2, 2)
            };
        }


        [Fact]
        public void Document_Is_Updated()
        {
            _responseModel = new Profile
            {
                Id = _model.Id,
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                Age = _model.Age,
                Created = _model.Created,
                Updated = _model.Updated
            };

            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            var response = _service.UpdateAsync(_modelToSave).Result;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Profile>()));
        }


        [Fact]
        public void Non_Existing_DocumentId_Causes_Error_Response()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(default(Profile)));

            var response = _service.UpdateAsync(_modelToSave).Result;

            Assert.Equal(StatusCode.NotFound, response.StatusCode);
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Crash_Causes_Error_Response()
        {
            _responseModel = new Profile();
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_responseModel));

            _repoMock.Setup(x => x.UpdateAsync(_model)).ThrowsAsync(new Exception("Error message"));

            var response = _service.UpdateAsync(_model).Result as BaseErrorResponse;

            Assert.ThrowsAsync<Exception>(() => _repoMock.Object.UpdateAsync(It.IsAny<Profile>()));

            Assert.Equal(StatusCode.Error, response.StatusCode);
        }
    }
}
