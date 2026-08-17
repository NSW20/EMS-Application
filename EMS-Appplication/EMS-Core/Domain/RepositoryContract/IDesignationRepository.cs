using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDesignationRepository
    {
        Task<IEnumerable<Designation>> GetDesignationAsync();
        Task<Designation> GetDesignationByIdAsync(int id);
        Task<Designation> AddDesignationAsync(Designation designation);
        Task<Designation> UpdateDesignationAsync(Designation designation,int designationId);
        Task<bool> DeleteDesignationAsync(int designationId);
    }
}
