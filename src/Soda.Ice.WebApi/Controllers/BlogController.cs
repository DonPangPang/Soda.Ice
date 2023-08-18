using Microsoft.AspNetCore.Mvc;
using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class BlogController : ApiControllerBase<Blog, VBlog, BlogParameters>
{
    public BlogController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}