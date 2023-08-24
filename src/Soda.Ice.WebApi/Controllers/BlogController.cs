using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soda.AutoMapper;
using Soda.Ice.Abstracts;
using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.Extensions;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class BlogController : ApiControllerBase<Blog, VBlog, BlogParameters>
{
    private readonly IUnitOfWork _unitOfWork;

    public BlogController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] BlogSearchParameters blogParameters)
    {
        var res = await _unitOfWork.Query<Blog>()
            .WhereIf(string.IsNullOrWhiteSpace(blogParameters.SearchKey),
                x => x.Title.Contains(blogParameters.SearchKey!) ||
                    x.Descrption.Contains(blogParameters.SearchKey!))
            .Map<Blog, VBlogTiny>().QueryAsync(blogParameters);

        return Success(res);
    }
}