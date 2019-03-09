using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class UpdatedResponse<TEntity> : BaseCrudResponse, IUpdateResponse<TEntity>
    {
        public TEntity Data { get; set; }
        public UpdatedResponse(TEntity entity)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Data = entity;
        }
    }


    public class UpdateNotFoundResponse<TEntity> : BaseNotFoundResponse, IUpdateResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public UpdateNotFoundResponse(object id, TEntity data) : base(id)
        {
            Data = data;
        }
    }


    public class UpdateErrorResponse<TEntity> : BaseErrorResponse, IUpdateResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public UpdateErrorResponse(TEntity data, string message, Exception ex = null) : base(message, ex)
        {
            Data = data;
        }
    }
}
