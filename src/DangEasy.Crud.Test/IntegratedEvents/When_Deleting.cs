using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;
using DangEasy.Crud.Interfaces;
using DangEasy.Crud.Events;
using DangEasy.Crud.Test.Unit;

namespace DangEasy.Crud.Test.IntegratedEvents
{
    public class When_Deleting : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;

        public When_Deleting() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }

        public override void Dispose()
        {
            DomainEvents.Remove(typeof(MyDeleteStartMethodEvent));
        }



        [Fact]
        public void Start_Event_Is_Executed()
        {
            DomainEvents.Register<DeleteStartMethodEvent<Profile>>(new MyDeleteStartMethodEvent());

            var response = _service.DeleteAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse);
            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<object>()), Times.Never);
        }



        [Fact]
        public void Success_Event_Is_Executed()
        {
            DomainEvents.Register<DeleteSuccessEvent<Profile>>(new MyDeleteSuccessHandler());

            _repoMock.Setup(x => x.GetByIdAsync("123")).Returns(Task.FromResult(_model));
            _repoMock.Setup(x => x.DeleteAsync("123")).Returns(Task.FromResult(true));

            var response = _service.DeleteAsync("123").Result;

            Assert.Equal(typeof(MyCustomResponse), response.GetType());
            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<object>()));
        }



        public class MyCustomResponse : DeletedResponse, IDeleteResponse
        {
            public MyCustomResponse(object id) : base(id)
            {
                StatusCode = Enums.StatusCode.Error;
            }
        }

        public class MyDeleteStartMethodEvent : DeleteStartMethodEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (DeleteStartMethodEvent<Profile>)args;
                Assert.NotNull(eventArgs.Id);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse(eventArgs.Id)
                };
            }
        }



        public class MyDeleteSuccessHandler : DeleteSuccessEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (DeleteSuccessEvent<Profile>)args;
                Assert.NotNull(eventArgs.Id);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse(eventArgs.Id)
                };
            }
        }
    }
}
