using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Soda.Ice.WebApi.Setups;

public static class JsonSetups
{
    public static void AddJson(this IMvcBuilder builder)
    {
        builder.AddNewtonsoftJson(setup =>
        {
            setup.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();
            setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        })
        /*添加XML*/.AddXmlDataContractSerializerFormatters()
        .ConfigureApiBehaviorOptions(setup =>
        {
            setup.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "http://www.baidu.com",
                    Title = "有错误",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = "请看详细信息",
                    Instance = context.HttpContext.Request.Path
                };

                problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });
    }
}