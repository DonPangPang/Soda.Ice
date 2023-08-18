using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class BlogTagController : ApiControllerBase<BlogTag, VBlogTag, BlogTagParameters>
{
    public BlogTagController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}