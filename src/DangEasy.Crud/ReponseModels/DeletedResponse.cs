using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class DeletedResponse : BaseCrudResponse, IDeleteResponse
    {
        public object Id { get; set; }

        public DeletedResponse(object id)
        {
            StatusCode = StatusCode.NoContent;
            Success = true;
            Id = id;
        }
    }

    public class DeleteNotFoundResponse : BaseNotFoundResponse, IDeleteResponse
    {
        public DeleteNotFoundResponse(object id) : base(id)
        {
        }
    }


    public class DeleteErrorResponse : BaseErrorResponse, IDeleteResponse
    {
        public object Id { get; set; }

        public DeleteErrorResponse(object id, string message, Exception ex = null) : base(message, ex)
        {
            Id = id;
        }
    }
}
