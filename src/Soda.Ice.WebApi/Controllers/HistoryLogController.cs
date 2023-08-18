using Soda.Ice.Domain;
using Soda.Ice.Shared.Parameters;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Controllers;

public class HistoryLogController : ApiControllerBase<HistoryLog, VHistoryLog, HistoryLogParameters>
{
    public HistoryLogController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}