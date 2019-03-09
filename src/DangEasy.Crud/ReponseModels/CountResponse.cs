using System;
using DangEasy.Crud.Enums;
using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.ResponseModels
{
    public class CountResponse : BaseCrudResponse, ICountResponse
    {
        public int? Count { get; set; }

        public CountResponse(int count)
        {
            StatusCode = StatusCode.Ok;
            Success = true;
            Count = count;
        }
    }

    public class CountErrorResponse : BaseErrorResponse, ICountResponse
    {
        public int? Count { get; set; }

        public CountErrorResponse(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
