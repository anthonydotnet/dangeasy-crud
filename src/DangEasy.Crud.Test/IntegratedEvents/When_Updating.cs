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
    [Collection("NonParallel")]
    public class When_Updating : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;

        public When_Updating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }

        public override void Dispose()
        {
            DomainEvents.Remove(typeof(MyUpdateStartMethodHandler));
        }



        [Fact]
        public void Start_Event_Is_Executed()
        {
            DomainEvents.Register<UpdateStartMethodEvent<Profile>>(new MyUpdateStartMethodHandler());

            var response = _service.UpdateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Never);
        }



        [Fact]
        public void Success_Event_Is_Executed()
        {
            DomainEvents.Register<UpdateSuccessEvent<Profile>>(new MyUpdateSuccessHandler());

            _repoMock.Setup(x => x.GetByIdAsync("123")).Returns(Task.FromResult(_model));

            var response = _service.UpdateAsync(new Profile { Id = "123" }).Result;

            Assert.Equal(typeof(MyCustomResponse<Profile>), response.GetType());
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Profile>()));
        }




        public class MyCustomResponse<Profile> : UpdatedResponse<Profile>, IUpdateResponse<Profile>
        {
            public MyCustomResponse(object id, Profile profile) : base(profile)
            {
                StatusCode = Enums.StatusCode.Ok;
            }
        }

        public class MyUpdateStartMethodHandler : UpdateStartMethodEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (UpdateStartMethodEvent<Profile>)args;
                Assert.NotNull(eventArgs.Entity);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse<Profile>(eventArgs.Entity.Id, eventArgs.Entity)
                };
            }
        }



        public class MyUpdateSuccessHandler : UpdateSuccessEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (UpdateSuccessEvent<Profile>)args;
                Assert.NotNull(eventArgs.Entity);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse<Profile>(eventArgs.Entity.Id, eventArgs.Entity)
                };
            }
        }
    }
}
