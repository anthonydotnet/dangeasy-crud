using System;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.Test.Fakes
{
    class MyDateTime : IDateTimeProvider
    {
        public DateTime Now => new DateTime(2019, 1, 1);

        public DateTime UtcNow => new DateTime(2019, 1, 1);
    }
}
