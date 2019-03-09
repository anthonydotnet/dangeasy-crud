using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class CreateResponse<TEntity> : BaseCrudResponse, ICreateResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public CreateResponse(TEntity entity)
        {
            StatusCode = StatusCode.Created;
            Success = true;
            Data = entity;
        }
    }


    //--
    // Error / unexpected responses
    //--

    // used when creating with existing ID
    public class ConflictResponse<TEntity> : BaseCrudResponse, ICreateResponse<TEntity>
    {
        public object Id { get; set; }
        public TEntity Data { get; set; }

        public ConflictResponse(object id, TEntity model)
        {
            StatusCode = StatusCode.Conflict;
            Success = false;
            Id = id;
            Data = model;
        }
    }


    public class CreateErrorResponse<TEntity> : BaseErrorResponse, ICreateResponse<TEntity>
    {
        public TEntity Data { get; set; }

        public CreateErrorResponse(TEntity data, string message, Exception ex = null) : base(message, ex)
        {
            Data = data;
        }
    }
}
