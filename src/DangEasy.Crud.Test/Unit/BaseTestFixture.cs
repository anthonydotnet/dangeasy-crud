using System;
using DangEasy.Crud.Test.Unit.Models;

namespace DangEasy.Crud.Test.Unit
{
    public class BaseTestFixture : IDisposable
    {
        protected readonly Profile _model;
        public BaseTestFixture()
        {
            // inset a document
            _model = new Profile
            {
                Id = "Model.1",
                FirstName = "Anthony",
                LastName = "Dang",
                Age = 39,
                Created = new DateTime(2019, 1, 1),
                Updated = new DateTime(2019, 1, 1)
            };
        }


        public virtual void Dispose()
        {
        }
    }
}
