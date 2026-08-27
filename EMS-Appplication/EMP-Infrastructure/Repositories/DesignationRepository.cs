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
