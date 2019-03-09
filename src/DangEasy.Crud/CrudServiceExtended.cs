using System.Threading.Tasks;
using DangEasy.Crud.Results;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Reflection;
using System.Linq.Expressions;
using System;
using System.Diagnostics;

namespace DangEasy.Crud
{
    public class CrudServiceExtended<TEntity> : CrudService<TEntity>
    where TEntity : class
    {
        public CrudServiceExtended(IRepositoryExtended<TEntity> repository, Expression<Func<TEntity, object>> idNameFactory = null)
          : base(repository, idNameFactory)
        {

        }



        public async Task<ICrudResult> QueryAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //-- Event: Method start

            try
            {
                var docs = await ((IRepositoryExtended<TEntity>)_repository).QueryAsync(predicate);

                //-- Event: Successful execution
                if (docs != null)
                {
                    return new QueryResult<TEntity>(docs);
                }

                return new NotFoundResult(predicate);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new ErrorResult($"QueryBySql error. {ex.Message}", ex.StackTrace);
            }
        }


        public async Task<ICrudResult> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //-- Event: Method start

            try
            {
                var docs = await ((IRepositoryExtended<TEntity>)_repository).FirstOrDefaultAsync(predicate);

                //-- Event: Successful execution
                if (docs != null)
                {
                    return new QueryResult<TEntity>(docs);
                }

                return new NotFoundResult(predicate);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new ErrorResult($"FirstOrDefaultAsync error. {ex.Message}", ex.StackTrace);
            }
        }


        public async Task<ICrudResult> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //-- Event: Method start

            try
            {
                var count = await ((IRepositoryExtended<TEntity>)_repository).CountAsync(predicate);

                //-- Event: Successful execution
                return new CountResult(count);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new ErrorResult($"CountAsync By Sql error. {ex.Message}", ex.StackTrace);
            }
        }
    }
}
