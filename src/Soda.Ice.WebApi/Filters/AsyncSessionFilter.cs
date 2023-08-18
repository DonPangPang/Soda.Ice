using Microsoft.AspNetCore.Mvc.Filters;
using Soda.Ice.WebApi.Services.CurrentUserServices;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Filters
{
    public class AsyncSessionFilter : IAsyncActionFilter
    {
        private readonly Session _session;
        private readonly IUnitOfWork _unitOfWork;

        public AsyncSessionFilter(Session session, IUnitOfWork unitOfWork)
        {
            _session = session;
            _unitOfWork = unitOfWork;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}