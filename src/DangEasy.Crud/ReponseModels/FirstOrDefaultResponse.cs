using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class FirstOrDefaultResponse<TEntity> : BaseCrudResponse, IFirstOrDefaultResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public FirstOrDefaultResponse(TEntity entity)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Data = entity;
        }
    }


    public class FirstOrDefaultErrorResponse<TEntity> : BaseErrorResponse, IFirstOrDefaultResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public FirstOrDefaultErrorResponse(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
