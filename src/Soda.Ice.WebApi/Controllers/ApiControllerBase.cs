using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soda.AutoMapper;
using Soda.Ice.Abstracts;
using Microsoft.EntityFrameworkCore;
using Soda.Ice.WebApi.UnitOfWorks;
using Soda.Ice.WebApi.Extensions;
using Soda.Ice.Shared;

namespace Soda.Ice.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class ApiControllerBase : ControllerBase
    {
        [NonAction]
        public IActionResult Success(string? message, object? data = null)
        {
            return Ok(new IceResponse
            {
                IsSuccess = true,
                Message = message,
                Data = data
            });
        }

        [NonAction]
        public IActionResult Success(object? data = null)
        {
            return Ok(new IceResponse
            {
                IsSuccess = true,
                Data = data
            });
        }

        [NonAction]
        public IActionResult Fail(string? message, object? data = null)
        {
            return Ok(new IceResponse
            {
                IsSuccess = false,
                Message = message,
                Data = data
            });
        }

        [NonAction]
        public IActionResult Fail(object? data = null)
        {
            return Ok(new IceResponse
            {
                IsSuccess = false,
                Data = data
            });
        }
    }

    public abstract class ApiControllerBase<TEntity, TViewModel, TParameters> : ApiControllerBase
        where TEntity : class, IEntity
        where TViewModel : class, IViewModel
        where TParameters : IceParameters
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] TParameters parameters)
        {
            var res = await _unitOfWork.Query<TEntity>().Map<TEntity, TViewModel>().QueryAsync(parameters);

            return Success(res);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Fail("Not Found.");
            }

            var res = await _unitOfWork.Query<TEntity>().Map<TEntity, TViewModel>().FirstOrDefaultAsync();

            return res is null ? Fail("Not Found.") : Success(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), $"{nameof(model)} 不能为空.");
            }

            var entity = await _unitOfWork.Query<TEntity>().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity is null)
            {
                return Fail("Not Found.");
            }

            entity.Map(model);

            _unitOfWork.Db.Update(entity);

            if (await _unitOfWork.CommitAsync())
            {
                return Success();
            }

            return Fail();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), $"{nameof(model)} 不能为空.");
            }

            var entity = await _unitOfWork.Query<TEntity>().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity is not null)
            {
                return Fail("Not Found.");
            }

            entity = model.MapTo<TEntity>();

            _unitOfWork.Db.Add(entity);

            if (await _unitOfWork.CommitAsync())
            {
                return Ok();
            }

            return Fail();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Query<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                return Fail("Not Found.");
            }

            _unitOfWork.Db.Remove(entity);

            if (await _unitOfWork.CommitAsync())
            {
                return Success();
            }

            return Fail("删除失败");
        }
    }
}