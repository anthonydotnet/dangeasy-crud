using System;
using Xunit;
using DangEasy.Crud.Test.Unit.Models;
using Moq;
using DangEasy.Interfaces.Database;
using System.Threading.Tasks;
using DangEasy.Crud.ResponseModels;
using DangEasy.Crud.Interfaces;
using DangEasy.Crud.Test.Unit;
using DangEasy.Crud.Events;

namespace DangEasy.Crud.Test.IntegratedEvents
{
    [Collection("NonParallel")]
    public class When_Creating : BaseTestFixture, IDisposable
    {
        CrudService<Profile> _service;
        Mock<IRepository<Profile>> _repoMock;

        public When_Creating() : base()
        {
            _repoMock = new Mock<IRepository<Profile>>();
            _service = new CrudService<Profile>(_repoMock.Object);
        }

        public override void Dispose()
        {
            DomainEvents.Remove(typeof(MyCreateStartMethodHandler));
        }


        // TODO: what about testing for NOT exiting the method?
        // Should a dev be allowed to CANCEL a conflict?



        [Fact]
        public void Start_Event_Is_Executed()
        {
            DomainEvents.Register<CreateStartMethodEvent<Profile>>(new MyCreateStartMethodHandler());

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.NotNull(response as MyCustomResponse<Profile>);
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()), Times.Never);
        }


        [Fact]
        public void Success_Event_Is_Executed()
        {
            DomainEvents.Register<CreateSuccessEvent<Profile>>(new MyCreateSuccessHandler());

            // _repoMock.Setup(x => x.GetByIdAsync("123")).Returns(Task.FromResult(_model));

            var response = _service.CreateAsync(new Profile { Id = "123" }).Result;

            Assert.Equal(typeof(MyCustomResponse<Profile>), response.GetType());
            _repoMock.Verify(x => x.CreateAsync(It.IsAny<Profile>()));
        }



        public class MyCustomResponse<Profile> : CreateResponse<Profile>, ICreateResponse<Profile>
        {
            public MyCustomResponse(Profile profile) : base(profile)
            {
                StatusCode = Enums.StatusCode.Ok;
            }
        }

        public class MyCreateStartMethodHandler : CreateStartMethodEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (CreateStartMethodEvent<Profile>)args;
                Assert.NotNull(eventArgs.Entity);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse<Profile>(eventArgs.Entity)
                };
            }
        }


        public class MyCreateSuccessHandler : CreateSuccessEvent<Profile>, IHandles<IDomainEvent>
        {
            public EventResult Handle(IDomainEvent args)
            {
                var eventArgs = (CreateSuccessEvent<Profile>)args;
                Assert.NotNull(eventArgs.Entity);
                Assert.NotNull(eventArgs.Repository);

                // do some custom logic here!

                return new EventResult
                {
                    ExitMethod = true,
                    CrudResponse = new MyCustomResponse<Profile>(eventArgs.Entity)
                };
            }
        }
    }
}
