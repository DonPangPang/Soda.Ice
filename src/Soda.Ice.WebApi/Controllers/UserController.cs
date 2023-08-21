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

public class UserController : ApiControllerBase<User, VUser, UserParameters>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}