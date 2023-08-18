using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class BlogViewLogController : ApiControllerBase<BlogViewLog, VBlogViewLog, BlogViewLogParameters>
{
    public BlogViewLogController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}