using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class UserController : ApiControllerBase<User, VUser, UserParameters>
{
    public UserController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}