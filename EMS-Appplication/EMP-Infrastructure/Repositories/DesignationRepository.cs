using EMP_Infrastructure.SqlOperation;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMP_Infrastructure.Repositories
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly EMSDbContext eMSDbContext;
        private readonly ILogger<Designation> logger;
        public DesignationRepository(EMSDbContext eMSDbContext, ILogger<Designation> logger)
        {
            this.eMSDbContext = eMSDbContext;
            this.logger = logger;
        }
        public async Task<Designation> AddDesignationAsync(Designation designation, CancellationToken token)
        {
            logger.LogInformation("{method}.{class}.Requested receive to Add a designation", nameof(AddDesignationAsync), nameof(DesignationRepository));
            await eMSDbContext.Designations.AddAsync(designation);
            await eMSDbContext.SaveChangesAsync(token);
            return designation;
        }

        public async Task<bool> DeleteDesignationAsync(int designationId, CancellationToken token)
        {
            logger.LogInformation("{method}.{class}.Requested receive to delete a designation", nameof(DeleteDesignationAsync), nameof(DesignationRepository));
            var desginationToDelete = await eMSDbContext.Designations.FirstOrDefaultAsync(x => x.DesignationId == designationId);

            eMSDbContext.Designations.Remove(desginationToDelete);
            await eMSDbContext.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Designation>> GetDesignationAsync(CancellationToken token)
        {
            logger.LogInformation("{method}.{class}.Requested receive to fetch all designation", nameof(GetDesignationAsync), nameof(DesignationRepository));
            return await eMSDbContext.Designations.ToListAsync(token);
        }

        public async Task<(IEnumerable<Designation>, int)> GetPagginatedDesignationAsync(CancellationToken token, string? searchText, string sortOrder = "ASC", string sortColumn = "Title", int pageNumber = 1, int pageSize = 10)
        {
            logger.LogInformation("{method}.{class}.Requested receive to fetch all designation", nameof(GetPagginatedDesignationAsync), nameof(DesignationRepository));
            var query = eMSDbContext.Designations.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => x.Title.Contains(searchText));
            }
            query = sortColumn.ToLower() switch
            {
                "title" => sortOrder.Equals("DESC", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                "designationid" => sortOrder.Equals("DESC", StringComparison.OrdinalIgnoreCase) ?
                query.OrderByDescending(x => x.DesignationId) : query.OrderBy(x => x.DesignationId),
                _=>query.OrderBy(x=>x.Title)
            };
            var totalColumnCount = await query.CountAsync();
            var paginatedResult = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (paginatedResult, totalColumnCount);
           
        }

        public async Task<Designation> GetDesignationByIdAsync(int id, CancellationToken token)
        {
            logger.LogInformation("{method}.{class}.Requested receive to fetch a designation", nameof(GetDesignationByIdAsync), nameof(DesignationRepository));
            var desgination = await eMSDbContext.Designations.FirstOrDefaultAsync(x => x.DesignationId == id,token);
            return desgination;
        }

        public async Task<Designation> UpdateDesignationAsync(Designation designation, int designationId, CancellationToken token)
        {
            logger.LogInformation("{method}.{class}.Requested receive to update a designation", nameof(UpdateDesignationAsync), nameof(DesignationRepository));
            var desginationToUpdate = await eMSDbContext.Designations.FirstOrDefaultAsync(x => x.DesignationId == designationId);
            desginationToUpdate.Title = designation.Title;
            desginationToUpdate.DepartmentId = designation.DepartmentId;
            eMSDbContext.Designations.Update(desginationToUpdate);
            await eMSDbContext.SaveChangesAsync(token);
            return desginationToUpdate;
        }
    }
}
