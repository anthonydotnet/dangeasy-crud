using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.Test.Unit.Events
{
    public class When_Creating : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;

        public When_Creating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }


        // TODO: what about testing for NOT exiting the method?
        // Should a dev be allowed to CANCEL a conflict?



        [Fact]
        public void Start_Event_Is_Executed()
        {
            _service.OnCreate_MethodStart += _service_OnCreate_CustomEvent;

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Conflict_Event_Is_Executed()
        {
            _service.OnCreate_HasConflict += _service_OnCreate_CustomEvent;

            _repoMock.Setup(x => x.GetByIdAsync("123")).Returns(Task.FromResult(_model));

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void NoConflict_Event_Is_Executed()
        {
            _service.OnCreate_NoConflict += _service_OnCreate_CustomEvent;

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Success_Event_Is_Executed()
        {
            _service.OnCreate_Success += _service_OnCreate_CustomEvent;

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
        }


        [Fact]
        public void Exception_Event_Is_Executed()
        {
            _service.OnCreate_Exception += _service_OnCreate_CustomEvent;

            _repoMock.Setup(x => x.CreateAsync(It.IsAny<Profile>())).Throws(new Exception("Test"));
            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
        }


        //--
        // Events 
        //--
        ICreateResponse<Profile> _service_OnCreate_CustomEvent(IRepository<Profile> repository, Profile entity, out bool exitMethod)
        {
            // do something, then return a custom response which involves ending the method

            exitMethod = true;

            return new MyCustomResponse<Profile>(entity.Id, entity);
        }


        public class MyCustomResponse<Profile> : CreateResponse<Profile>, ICreateResponse<Profile>
        {
            public MyCustomResponse(object id, Profile profile) : base(profile)
            {
            }
        }
    }
}
