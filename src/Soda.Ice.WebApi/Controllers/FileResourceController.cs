using Microsoft.AspNetCore.Mvc;
using Soda.Ice.Domain;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class FileResourceController : ApiControllerBase<FileResource, VFileResource, FileResourceParameters>
{
    public FileResourceController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        return Success();
    }
}