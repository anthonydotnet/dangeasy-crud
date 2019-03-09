using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;

namespace DangEasy.Crud.Test.Unit
{
    public class When_Creating : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;
        Profile _responseModel;

        public When_Creating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }

        [Fact]
        public void Valid_Data_Is_Persisted()
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

            _repoMock.Setup(x => x.CreateAsync(_model)).Returns(Task.FromResult(_responseModel));

            var response = _service.CreateAsync(_model).Result as CreateResponse<Profile>;

            Assert.Equal(StatusCode.Created, response.StatusCode);

            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()));

            Assert.Equal(_model.Id, response.Data.Id);
            Assert.Equal(_model.FirstName, response.Data.FirstName);
            Assert.Equal(_model.AccountId, response.Data.AccountId);
            Assert.Equal(_model.Created, response.Data.Created);
            Assert.Equal(_model.Updated, response.Data.Updated);
        }

        [Fact]
        public void Duplicate_Id_Causes_A_Bad_Response()
        {
            var conflictModel = new Profile
            {
                Id = _model.Id
            };

            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(conflictModel));

            var response = _service.CreateAsync(_model).Result as ConflictResponse<Profile>;

            Assert.Equal(StatusCode.Conflict, response.StatusCode);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Data_With_Custom_IdFactory_Is_Persisted()
        {
            _service = new CrudService<Profile>(_repoMock.Object, x => x.Id);
            Valid_Data_Is_Persisted();
        }


        [Fact]
        public void Crash_Causes_Error_Response()
        {
            _responseModel = new Profile();

            _repoMock.Setup(x => x.CreateAsync(_model)).ThrowsAsync(new Exception("Error message"));

            var response = _service.CreateAsync(_model).Result as BaseErrorResponse;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.ThrowsAsync<Exception>(() => _repoMock.Object.CreateAsync(It.IsAny<Profile>()));
        }
    }
}
