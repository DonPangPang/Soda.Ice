using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class LoginLogController : ApiControllerBase<LoginLog, VLoginLog, LoginLogParameters>
{
    public LoginLogController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}