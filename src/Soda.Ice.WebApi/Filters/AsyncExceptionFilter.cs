using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using Soda.Ice.Shared;

namespace Soda.Ice.WebApi.Filters;

public class AsyncExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        OkObjectResult result = new OkObjectResult(new IceResponse
        {
            IsSuccess = false,
            Message = "服务器异常.",
            Data = context.Exception.Message
        });

        context.Result = result;
        context.ExceptionHandled = true;

        return Task.FromResult(result);
    }
}