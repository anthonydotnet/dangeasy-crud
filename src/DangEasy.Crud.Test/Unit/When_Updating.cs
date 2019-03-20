using Xunit;
using System;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using System.Threading.Tasks;
using DangEasy.Crud.Enums;
using DangEasy.Crud.ResponseModels;
using DangEasy.Crud.Test.Fakes;

namespace DangEasy.Crud.Test.Unit
{
    public class When_Updating : BaseTestFixture, IDisposable
    {
        private Mock<IRepository<Profile>> _repoMock;
        private CrudService<Profile> _service;
        private Profile _responseModel;
        private MyDateTime _myDateTime;

        public When_Updating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _myDateTime = new MyDateTime();
            _service = new CrudService<Profile>(_repoMock.Object, dateTimeProvider: _myDateTime);
        }


        [Fact]
        public void Document_Is_Updated()
        {
            var shouldBeUnchangedDate = _myDateTime.UtcNow.AddDays(-1);

            _responseModel = new Profile
            {
                Id = _model.Id,
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                Age = _model.Age,
                Created = shouldBeUnchangedDate,
                Updated = _myDateTime.UtcNow
            };

            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(_model));
            _repoMock.Setup(x => x.UpdateAsync(_model)).Returns(Task.FromResult(_responseModel));

            var response = _service.UpdateAsync(_model).Result;

            Assert.Equal(StatusCode.Ok, response.StatusCode);
            _repoMock.Verify(x => x.UpdateAsync(_model));

            // ensure only updated date was changed
            Assert.Equal(shouldBeUnchangedDate, _responseModel.Created);

            // assert that the ORIGINAL model had it's date changed
            Assert.Equal(_myDateTime.UtcNow, _model.Updated);
        }


        [Fact]
        public void Non_Existing_DocumentId_Causes_Error_Response()
        {
            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(default(Profile)));

            var response = _service.UpdateAsync(_model).Result;

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
