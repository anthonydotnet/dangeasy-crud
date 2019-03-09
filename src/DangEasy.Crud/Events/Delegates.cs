
using System;
using DangEasy.Crud.Interfaces;
using DangEasy.Interfaces.Database;

namespace DangEasy.Crud.Events
{
    public class Delegates
    {
        public delegate ICreateResponse<TEntity> CreateDelegate<TEntity>(IRepository<TEntity> repository, TEntity entity, out bool exitMethod) where TEntity : class;

        public delegate IUpdateResponse<TEntity> UpdateDelegate<TEntity>(IRepository<TEntity> repository, TEntity entity, out bool exitMethod) where TEntity : class;

    }
}
