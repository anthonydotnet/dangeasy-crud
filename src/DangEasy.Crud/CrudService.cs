using System.Threading.Tasks;
using DangEasy.Crud.Results;
using DangEasy.Interfaces.Database;
using DangEasy.Crud.Reflection;
using System.Linq.Expressions;
using System;
using System.Diagnostics;
using DangEasy.Crud.Interfaces;
using DangEasy.Crud.ResponseModels;
using System.Linq;

namespace DangEasy.Crud
{
    public class CrudService<TEntity> where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly string _repoIdentityProperty;

        public CrudService(IRepository<TEntity> repository, Expression<Func<TEntity, object>> idNameFactory = null)
        {
            _repository = repository;
            _repoIdentityProperty = ReflectionHelper.TryGetIdProperty(idNameFactory);
        }


        public async Task<ICreateResponse<TEntity>> CreateAsync(TEntity poco)
        {
            //-- Event: Method start 

            try
            {
                var entityId = ReflectionHelper.GetPropertyValue(poco, _repoIdentityProperty);

                if (!string.IsNullOrWhiteSpace(entityId as string))
                {
                    // check for existance first!
                    var doc = await _repository.GetByIdAsync(entityId);

                    if (doc != null)
                    {
                        //-- Event: Already exists 

                        return new ConflictResponse<TEntity>(entityId, poco);
                    }
                }

                //-- Event: Doc NOT exists, so create! 

                var res = await _repository.CreateAsync(poco);

                var response = new CreateResponse<TEntity>(res);

                //-- Event: Successful execution

                return response;
            }
            catch (Exception ex)
            {
                //-- Event: Error during create

                return new CreateErrorResponse<TEntity>(poco, ErrorMessage.Build("Create", poco.ToString()), ex);
            }
        }


        public async Task<IUpdateResponse<TEntity>> UpdateAsync(TEntity poco)
        {
            //-- Event: Method start 

            try
            {
                // check for existance!
                var entityId = ReflectionHelper.GetPropertyValue(poco, _repoIdentityProperty);

                var doc = await _repository.GetByIdAsync(entityId);
                if (doc == null)
                {
                    //-- Event: Does not exist

                    return new UpdateNotFoundResponse<TEntity>(entityId, poco);
                }

                //-- Event: Doc exists - Pre-Update
                var res = await _repository.UpdateAsync(poco);

                var response = new UpdatedResponse<TEntity>(res);

                //-- Event: Successful execution 

                return response;
            }
            catch (Exception ex)
            {
                //-- Event: Error during update

                return new UpdateErrorResponse<TEntity>(poco, ErrorMessage.Build("Update", poco.ToString()), ex);
            }
        }


        public async Task<IDeleteResponse> DeleteAsync(object entityId)
        {
            //-- Event: Method start

            try
            {
                var doc = await _repository.GetByIdAsync(entityId);
                if (doc == null)
                {
                    //-- Event: Does not exist

                    return new DeleteNotFoundResponse(entityId);
                }


                //-- Event: Does exist

                var success = await _repository.DeleteAsync(entityId);

                if (success)
                {
                    //-- Event: Successful execution


                    // No Content means success! Yeah that's strange! 
                    return new DeletedResponse(entityId);
                }

                //-- Event: Could not delete

                return new DeleteErrorResponse(entityId, $"Unknown error occured while trying to delete {entityId}");

            }
            catch (Exception ex)
            {
                //-- Event: Error during delete

                return new DeleteErrorResponse(entityId, ErrorMessage.Build("Delete", entityId), ex);
            }
        }




        //--
        // Count
        //--
        public async Task<ICountResponse> CountAsync()
        {
            //-- Event: Method start

            try
            {
                var count = await _repository.CountAsync();

                //-- Event: Successful execution

                return new CountResponse(count);

            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new CountErrorResponse(ErrorMessage.Build("Count"), ex);
            }
        }


        public async Task<ICountResponse> CountAsync(string sqlQuery)
        {
            //-- Event: Method start

            try
            {
                var count = await _repository.CountAsync(sqlQuery);

                //-- Event: Successful execution
                return new CountResponse(count);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new CountErrorResponse(ErrorMessage.Build("Count by Sql", sqlQuery), ex);
            }
        }


        //--
        // All the crazy GET methods!!!
        //--
        public async Task<IQueryResponse<TEntity>> GetAllAsync()
        {
            //-- Event: Method start

            try
            {
                var docs = await _repository.GetAllAsync();

                if (docs == null)
                {
                    throw new Exception($"Unknown error occured while trying to GetAll");
                }

                //-- Event: Successful execution

                return new QueryResponse<TEntity>(docs);

            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new QueryErrorResponse<TEntity>(ErrorMessage.Build("GetAll"), ex);
            }
        }


        public async Task<IQueryResponse<TEntity>> QueryAsync(string sqlQuery)
        {
            //-- Event: Method start

            try
            {
                var docs = await _repository.QueryAsync(sqlQuery);

                if (docs != null)
                {
                    //-- Event: Successful execution

                    return new QueryResponse<TEntity>(docs);
                }

                //-- Event: Not found - Will this actually be null ?
                return new QueryErrorResponse<TEntity>(sqlQuery);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new QueryErrorResponse<TEntity>(ErrorMessage.Build("QueryBySql", sqlQuery), ex);
            }
        }


        public async Task<IFirstOrDefaultResponse<TEntity>> FirstOrDefaultAsync(string sqlQuery)
        {
            //-- Event: Method start

            try
            {
                var doc = await _repository.FirstOrDefaultAsync(sqlQuery);

                //-- Event: Successful execution

                return new FirstOrDefaultResponse<TEntity>(doc);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new FirstOrDefaultErrorResponse<TEntity>(ErrorMessage.Build("FirstOrDefault", sqlQuery), ex);
            }
        }


        public async Task<IGetResponse<TEntity>> GetByIdAsync(object entityId)
        {
            //-- Event: Method start

            try
            {
                var doc = await _repository.GetByIdAsync(entityId);

                //-- Event: Successful execution
                if (doc != null)
                {
                    return new GetResponse<TEntity>(doc);
                }

                return new GetNotFoundResponse<TEntity>(entityId);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new GetErrorResponse<TEntity>(ErrorMessage.Build("GetById", entityId), ex);
            }
        }



        public async Task<ISprocResponse<TResult>> ExecuteStoredProcedureAsync<TResult>(string sprocName, params object[] parameters)
        {
            //-- Event: Method start

            try
            {
                var result = await _repository.ExecuteStoredProcedureAsync<TResult>(sprocName, parameters);

                //-- Event: Successful execution

                return new SprocResponse<TResult>(result);
            }
            catch (Exception ex)
            {
                //-- Event: Error during get all

                return new SprocErrorResponse<TResult>(ErrorMessage.Build("ExecuteStoredProcedure", sprocName, string.Join(", ", parameters), ex));
            }
        }
    }
}
