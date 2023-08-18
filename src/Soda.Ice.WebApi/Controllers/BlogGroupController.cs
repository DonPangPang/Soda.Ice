using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class BlogGroupController : ApiControllerBase<BlogGroup, VBlogGroup, BlogGroupParameters>
{
    public BlogGroupController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}