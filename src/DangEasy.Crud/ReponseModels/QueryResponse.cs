using System;
using System.Linq;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class QueryResponse<TEntity> : BaseCrudResponse, IQueryResponse<TEntity>
    {
        public IQueryable<TEntity> Data { get; set; }

        public QueryResponse(IQueryable<TEntity> entities)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Data = entities;
        }
    }


    public class QueryErrorResponse<TEntity> : BaseErrorResponse, IQueryResponse<TEntity>
    {
        public IQueryable<TEntity> Data { get; set; }

        public QueryErrorResponse(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
