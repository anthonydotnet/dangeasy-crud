using System.Linq;
using DangEasy.Crud.Enums;

namespace DangEasy.Crud.Interfaces
{
    public interface ICrudResponse
    {
        StatusCode StatusCode { get; set; }
        bool Success { get; set; }
    }


    public interface ICreateResponse<TEntity> : ICrudResponse
    {
        TEntity Data { get; set; }
    }

    public interface IUpdateResponse<TEntity> : ICrudResponse
    {
        TEntity Data { get; set; }
    }

    public interface IDeleteResponse : ICrudResponse
    {
        object Id { get; set; }
    }

    public interface IGetResponse<TEntity> : ICrudResponse
    {
        TEntity Data { get; set; }
    }

    public interface IQueryResponse<TEntity> : ICrudResponse
    {
        IQueryable<TEntity> Data { get; set; }
    }

    public interface IFirstOrDefaultResponse<TEntity> : ICrudResponse
    {
        TEntity Data { get; set; }
    }

    public interface ICountResponse : ICrudResponse
    {
        int? Count { get; set; }
    }

    public interface ISprocResponse<TResult> : ICrudResponse
    {
        TResult Data { get; set; }
    }
}
