using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDesignationRepository
    {
        Task<IEnumerable<Designation>> GetDesignationAsync(CancellationToken token);
        Task<(IEnumerable<Designation>,int)> GetPagginatedDesignationAsync(CancellationToken token,string? searchText,string sortOrder="ASC",string sortColumn= "Title", int pageNumber=1,int pageSize=10);
        Task<Designation> GetDesignationByIdAsync(int id, CancellationToken token);
        Task<Designation> AddDesignationAsync(Designation designation, CancellationToken token);
        Task<Designation> UpdateDesignationAsync(Designation designation,int designationId, CancellationToken token);
        Task<bool> DeleteDesignationAsync(int designationId, CancellationToken token);
    }
}
