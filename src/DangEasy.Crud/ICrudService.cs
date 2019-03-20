using System.Threading.Tasks;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud
{
    public interface ICrudService<TEntity> where TEntity : class
    {
        Task<ICountResponse> CountAsync();
        Task<ICountResponse> CountAsync(string sqlQuery);
        Task<ICreateResponse<TEntity>> CreateAsync(TEntity poco);
        Task<IDeleteResponse> DeleteAsync(object entityId);
        Task<ISprocResponse<TResult>> ExecuteStoredProcedureAsync<TResult>(string sprocName, params object[] parameters);
        Task<IFirstOrDefaultResponse<TEntity>> FirstOrDefaultAsync(string sqlQuery);
        Task<IQueryResponse<TEntity>> GetAllAsync();
        Task<IGetResponse<TEntity>> GetByIdAsync(object entityId);
        Task<IQueryResponse<TEntity>> QueryAsync(string sqlQuery);
        Task<IUpdateResponse<TEntity>> UpdateAsync(TEntity poco);
    }
}