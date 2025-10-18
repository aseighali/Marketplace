using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public bool IsSuccess => Success;
        public string? Message { get; set; }
        public string ErrorMessage => Message ?? string.Empty;
        public T? Data { get; set; }
        public T Value => Data!;

        public static Result<T> Ok(T data, string? message = null) =>
            new Result<T> { Success = true, Data = data, Message = message };

        public static Result<T> Fail(string message) =>
            new Result<T> { Success = false, Message = message };

        // Alternative static factory methods for compatibility
        public static Result<T> SuccessResult(T data) =>
            new Result<T> { Success = true, Data = data };

        public static Result<T> FailureResult(string message) =>
            new Result<T> { Success = false, Message = message };
    }
}
