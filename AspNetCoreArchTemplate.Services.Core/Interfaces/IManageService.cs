using PowerNutrition.Web.ViewModels.Manage;

namespace PowerNutrition.Services.Core.Interfaces
{
    public interface IManageService
    {
        Task<IEnumerable<AllSupplementsDeleteViewmodel>> GetAllSupplementsDeleteListAsync();
        Task<IEnumerable<AllSupplemenetsEditViewmodel>> GetAllSupplementsEditListAsync();

    }
}
