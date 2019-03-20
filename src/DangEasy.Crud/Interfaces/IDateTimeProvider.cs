using System;
namespace DangEasy.Crud.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
