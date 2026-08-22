using AutoMapper;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.Helpers;
using EMS_Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DesignationService> _logger;
        private readonly IDesignationRepository _designationRepository;
        public DesignationService(IMapper _mapper, ILogger<DesignationService> _logger, IDesignationRepository _designationRepository)
        {
            this._mapper = _mapper;
            this._logger = _logger;
            this._designationRepository = _designationRepository;
        }
        public async Task<DesignationDTO> AddDesignation(DesignationAddDTO designationAddDTO, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to Add a designation", nameof(AddDesignation), nameof(DesignationService));
            if(designationAddDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Input fields are incorrect.please try again", nameof(AddDesignation), nameof(DesignationService));
                throw new ArgumentNullException(nameof(designationAddDTO),"Please provide the required inputs");
            }
            var designation = _mapper.Map<Designation>(designationAddDTO);
            var result=await _designationRepository.AddDesignationAsync(designation, token);
            if (result is null)
            {

                _logger.LogError("{method}.{class}.Something went wrong while adding the designation.", nameof(AddDesignation), nameof(DesignationService));
                throw new InvalidOperationException("Something went wrong while adding the designation");
            }

            _logger.LogInformation("{method}.{class}.Designation added successfully.", nameof(AddDesignation), nameof(DesignationService));
            var resultToReturn = _mapper.Map<DesignationDTO>(result);
            return resultToReturn;
        }

        public async Task<bool> DeleteDesignationAsync(int designationId, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to delete a designation", nameof(DeleteDesignationAsync), nameof(DesignationService));

            if (designationId <= 0)
            {
                _logger.LogWarning("{method}.{class}.Please enter correct designationId", nameof(DeleteDesignationAsync), nameof(DesignationService));
                throw new ArgumentException("Please enter correct designationId", nameof(designationId));
            }
            var result = await _designationRepository.DeleteDesignationAsync(designationId, token);
            if (!result)
            {
                _logger.LogError("{method}.{class}.Something went wrong while deleting designation", nameof(DeleteDesignationAsync), nameof(DesignationService));
                throw new InvalidOperationException("Something went wrong while deleting designation");
            }
            _logger.LogInformation("{method}.{class}.Designation Deleted successfully", nameof(DeleteDesignationAsync), nameof(DesignationService));

            return result;
        }

        public async Task<IEnumerable<DesignationDTO>> GetAllDesignationAsync(CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to fetch all designation", nameof(GetAllDesignationAsync), nameof(DesignationService));
            var result = await _designationRepository.GetDesignationAsync(token);

            if (!result.Any())
            {
                _logger.LogInformation("{method}.{class}.No designations found", nameof(GetAllDesignationAsync), nameof(DesignationService));
                return Enumerable.Empty<DesignationDTO>();
            }
            var resultToReturn = _mapper.Map<IEnumerable<DesignationDTO>>(result);
            _logger.LogInformation("{method}.{class}.Designations fetched successfully", nameof(GetAllDesignationAsync), nameof(DesignationService));

            return resultToReturn;
        }

        public async Task<DesignationDTO> GetDesignationAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to fetch a designation", nameof(GetDesignationAsync), nameof(DesignationService));
            if (id <= 0)
            {
                _logger.LogWarning("{method}.{class}.Please provide a correct designation id", nameof(GetDesignationAsync), nameof(DesignationService));
                throw new ArgumentException("Please provide the correct input", nameof(id));
            }
            var result = await _designationRepository.GetDesignationByIdAsync(id, token);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.No designation found with id {id}", nameof(GetDesignationAsync), nameof(DesignationService),id);
                throw new KeyNotFoundException($"No designation found with id {id}");
            }
            var resultToBeReturned=_mapper.Map<DesignationDTO>(result);
            _logger.LogInformation("{method}.{class}.Designations fetched successfully", nameof(GetDesignationAsync), nameof(DesignationService));
            return resultToBeReturned;
        }

        public async Task<DesignationDTO> UpdateDesignation(DesignationDTO designationUpdateDTO, int designationId, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to update a designation", nameof(UpdateDesignation), nameof(DesignationService));
            if (designationId <= 0)
            {
                _logger.LogWarning("{method}.{class}.Please provide a correct designation id", nameof(UpdateDesignation), nameof(DesignationService));
                throw new ArgumentException("Please provide the correct input", nameof(designationId));
            }
            if (designationUpdateDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Please provide the required inputs", nameof(UpdateDesignation), nameof(DesignationService));
                throw new ArgumentNullException(nameof(designationUpdateDTO), "Please provide the required inputs");
            }
            var designationToUpdate=_mapper.Map<Designation>(designationUpdateDTO);
            var result = await _designationRepository.UpdateDesignationAsync(designationToUpdate, designationId, token);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.Something went wrong while updating designation", nameof(UpdateDesignation), nameof(DesignationService));
                throw new InvalidOperationException("Something went wrong while updating designation");
            }
            var resultUpdate = _mapper.Map<DesignationDTO>(result);
            _logger.LogInformation("{method}.{class}.Designations updated successfully", nameof(UpdateDesignation), nameof(DesignationService));
            return resultUpdate;

        }
    }
}
