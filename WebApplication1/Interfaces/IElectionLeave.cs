using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Interfaces
{
    public interface IElectionLeave
    {
        Response GetElectionLeavePlan(ElectionLeaveRequestAPI requestAPI);
    }
}