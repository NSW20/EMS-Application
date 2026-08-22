using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ServiceContracts
{
    public interface IDesignationService
    {
        Task<IEnumerable<DesignationDTO>> GetAllDesignationAsync(CancellationToken token);
        Task<DesignationDTO> GetDesignationAsync(int id, CancellationToken token);
        Task<DesignationDTO> AddDesignation(DesignationAddDTO designationAddDTO, CancellationToken token);

        Task<DesignationDTO> UpdateDesignation(DesignationDTO ddesignationUpdateDTO, int designationId, CancellationToken token);
        Task<bool> DeleteDesignationAsync(int designationId, CancellationToken token);
    }
}
