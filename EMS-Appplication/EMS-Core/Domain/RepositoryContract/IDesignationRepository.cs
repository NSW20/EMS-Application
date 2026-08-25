using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDesignationRepository
    {
        Task<IEnumerable<Designation>> GetDesignationAsync(CancellationToken token);
        Task<Designation> GetDesignationByIdAsync(int id, CancellationToken token);
        Task<Designation> AddDesignationAsync(Designation designation, CancellationToken token);
        Task<Designation> UpdateDesignationAsync(Designation designation,int designationId, CancellationToken token);
        Task<bool> DeleteDesignationAsync(int designationId, CancellationToken token);
    }
}
