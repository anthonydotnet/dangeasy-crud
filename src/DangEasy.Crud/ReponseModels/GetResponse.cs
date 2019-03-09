using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class GetResponse<TEntity> : BaseCrudResponse, IGetResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public GetResponse(TEntity entity)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Data = entity;
        }
    }


    public class GetNotFoundResponse<TEntity> : BaseNotFoundResponse, IGetResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public GetNotFoundResponse(object id) : base(id)
        {
            Data = default(TEntity);
        }
    }


    public class GetErrorResponse<TEntity> : BaseErrorResponse, IGetResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public GetErrorResponse(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
