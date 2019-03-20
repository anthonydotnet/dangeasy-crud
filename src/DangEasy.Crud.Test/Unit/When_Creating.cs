using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Enums;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;
using DangEasy.Crud.Events;
using DangEasy.Crud.Interfaces;
using DangEasy.Crud.Test.Fakes;

namespace DangEasy.Crud.Test.Unit
{
    public class When_Creating : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;
        Profile _responseModel;
        IDateTimeProvider _myDateTime;

        public When_Creating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _myDateTime = new MyDateTime();
            _service = new CrudService<Profile>(_repoMock.Object, dateTimeProvider: _myDateTime);
        }

        [Fact]
        public void Valid_Data_Is_Persisted()
        {
            _repoMock.Setup(x => x.CreateAsync(_model)).Returns(Task.FromResult(_model));

            var response = _service.CreateAsync(_model).Result;

            Assert.Equal(StatusCode.Created, response.StatusCode);

            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()));

            Assert.Equal(_myDateTime.UtcNow, response.Data.Created);
            Assert.Equal(_myDateTime.UtcNow, response.Data.Updated);
        }


        [Fact]
        public void Duplicate_UniqueKey_Causes_A_Conflict_Response()
        {
            var conflictModel = new Profile();

            _repoMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<string>())).Returns(Task.FromResult(conflictModel));

            var response = _service.CreateAsync(_model).Result;

            Assert.Equal(StatusCode.Conflict, response.StatusCode);
            Assert.True(response is ConflictResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Duplicate_Id_Causes_A_Conflict_Response()
        {
            var conflictModel = new Profile();

            _repoMock.Setup(x => x.GetByIdAsync(_model.Id)).Returns(Task.FromResult(conflictModel));

            var response = _service.CreateAsync(_model).Result;

            Assert.Equal(StatusCode.Conflict, response.StatusCode);
            Assert.True(response is ConflictResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Data_With_Custom_IdFactory_Is_Persisted()
        {
            _service = new CrudService<Profile>(_repoMock.Object, x => x.Id, _myDateTime);
            Valid_Data_Is_Persisted();
        }


        [Fact]
        public void Crash_Causes_Error_Response()
        {
            _responseModel = new Profile();

            _repoMock.Setup(x => x.CreateAsync(_model)).ThrowsAsync(new Exception("Error message"));

            var response = _service.CreateAsync(_model).Result;

            Assert.Equal(StatusCode.Error, response.StatusCode);
            Assert.ThrowsAsync<Exception>(() => _repoMock.Object.CreateAsync(It.IsAny<Profile>()));
        }
    }
}
