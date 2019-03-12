using DangEasy.Interfaces.Database;

namespace DangEasy.Crud.Events
{
    public class CreateStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public TEntity Entity { get; internal set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

    public class UpdateStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public TEntity Entity { get; set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

    public class DeleteStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public object Id { get; set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

    //public class GetStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    //{
    //    public object Id { get; set; }
    //    public IRepository<TEntity> Repository { get; internal set; }
    //}

    //public class GetAllStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    //{
    //    public IRepository<TEntity> Repository { get; internal set; }
    //}

    //public class QueryStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    //{
    //    public string Sql { get; internal set; }
    //    public IRepository<TEntity> Repository { get; internal set; }
    //}

    //public class CountStartMethodEvent<TEntity> : IDomainEvent where TEntity : class
    //{
    //    public string Sql { get; internal set; }
    //    public IRepository<TEntity> Repository { get; internal set; }
    //}



    // successful execution
    public class CreateSuccessEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public TEntity Entity { get; internal set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

    public class UpdateSuccessEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public TEntity Entity { get; set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }

    public class DeleteSuccessEvent<TEntity> : IDomainEvent where TEntity : class
    {
        public object Id { get; set; }
        public IRepository<TEntity> Repository { get; internal set; }
    }
}
