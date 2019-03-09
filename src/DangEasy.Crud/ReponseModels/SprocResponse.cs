using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class SprocResponse<TResult> : BaseCrudResponse, ISprocResponse<TResult>
    {
        public TResult Data { get; set; }

        public SprocResponse(TResult result)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Data = result;
        }
    }


    public class SprocErrorResponse<TResult> : BaseErrorResponse, ISprocResponse<TResult>
    {
        public TResult Data { get; set; }

        public SprocErrorResponse(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
