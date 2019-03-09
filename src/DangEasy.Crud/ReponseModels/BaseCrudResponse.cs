using DangEasy.Crud.Enums;
using System;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public abstract class BaseCrudResponse : ICrudResponse
    {
        public bool Success { get; set; }
        public StatusCode StatusCode { get; set; }
    }


    public class BaseErrorResponse : BaseCrudResponse, ICrudResponse
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public BaseErrorResponse(string message, Exception ex = null)
        {
            StatusCode = StatusCode.Error;
            Success = false;
            Message = message;
            Exception = ex;
        }
    }


    public class BaseNotFoundResponse : BaseCrudResponse, ICrudResponse
    {
        public object Id { get; set; }

        public BaseNotFoundResponse(object id)
        {
            StatusCode = StatusCode.NotFound;
            Success = false;
            Id = id;
        }
    }
}
