﻿namespace PowerNutrition.Services.Core.Interfaces
{
    using PowerNutrition.Web.ViewModels.Supplement;
    public interface ISupplementService
    {
        Task<IEnumerable<AllSupplementsViewmodel>> GetAllSupplementsAsync();

        Task<DetailsSupplementViewmodel?> GetDetailsForSupplementAsync(string? id);

        Task<Guid?> PersistAddSupplementAsync(AddSupplementInputModel inputModel);

        Task<SupplementDeleteInputModel?> GetSupplementToDelete(string? supplementId);

        Task<bool> DeleteSupplement(SupplementDeleteInputModel inputModel);

        Task<SupplementEditInputModel?> GetSupplementForEditAsync(string? supplementId);

        Task<Guid?> PersistEditSupplementAsync(SupplementEditInputModel inputModel);
    }
}
