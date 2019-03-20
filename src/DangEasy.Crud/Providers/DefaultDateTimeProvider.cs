using System;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.Providers
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get => DateTime.Now; }

        public DateTime UtcNow { get => DateTime.UtcNow; }
    }
}
