using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Interfaces
{
    public interface IMasterData
    {
        Response GetDivisions(MasterDataRequestAPI requestAPI);

        Response GetDepartments(MasterDataRequestAPI requestAPI);

        Response GetLocations(MasterDataRequestAPI requestAPI);

        Response GetTowns(MasterDataRequestAPI requestAPI);

        Response GetDesignations(MasterDataRequestAPI requestAPI);

        Response GetEducationLevels(MasterDataRequestAPI requestAPI);
    }
}