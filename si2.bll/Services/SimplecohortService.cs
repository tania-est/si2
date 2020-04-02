using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using si2.bll.Dtos.Requests.Simplecohort;
using si2.bll.Dtos.Results.Simplecohort;
using si2.bll.Helpers.PagedList;
using si2.bll.Helpers.ResourceParameters;
using si2.dal.Entities;
using si2.dal.UnitOfWork;
using Si2.common.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static si2.common.Enums;

namespace si2.bll.Services
{
    public class SimplecohortService : ServiceBase, ISimplecohortService
    {
        public SimplecohortService(IUnitOfWork uow, IMapper mapper, ILogger<SimplecohortService> logger) : base(uow, mapper, logger)
        {
        }

        public async Task<SimplecohortDto> CreateSimplecohortAsync(CreateSimplecohortDto createSimplecohortDto, CancellationToken ct)
        {
            SimplecohortDto simplecohortDto = null;
            try
            {
                var simplecohortEntity = _mapper.Map<Simplecohort>(createSimplecohortDto);
                await _uow.Simplecohorts.AddAsync(simplecohortEntity, ct);
                await _uow.SaveChangesAsync(ct);
                simplecohortDto = _mapper.Map<SimplecohortDto>(simplecohortEntity);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, string.Empty);
            }
            return simplecohortDto;
        }

        public async Task<SimplecohortDto> UpdateSimplecohortAsync(Guid id, UpdateSimplecohortDto updateSimplecohortDto, CancellationToken ct)
        {
            SimplecohortDto simplecohortDto = null;
            
            var updatedEntity = _mapper.Map<Simplecohort>(updateSimplecohortDto);
            updatedEntity.Id = id;
            await _uow.Simplecohorts.UpdateAsync(updatedEntity, id, ct, updatedEntity.RowVersion);
            await _uow.SaveChangesAsync(ct);
            var simplecohortEntity = await _uow.Simplecohorts.GetAsync(id, ct);
            simplecohortDto = _mapper.Map<SimplecohortDto>(simplecohortEntity);
            
            return simplecohortDto;
        }

        public async Task<SimplecohortDto> PartialUpdateSimplecohortAsync(Guid id, UpdateSimplecohortDto updateSimplecohortDto, CancellationToken ct)
        {
            var simplecohortEntity = await _uow.Simplecohorts.GetAsync(id, ct);

            _mapper.Map(updateSimplecohortDto, simplecohortEntity);

            await _uow.Simplecohorts.UpdateAsync(simplecohortEntity, id, ct, simplecohortEntity.RowVersion);
            await _uow.SaveChangesAsync(ct);

            simplecohortEntity = await _uow.Simplecohorts.GetAsync(id, ct);
            var simplecohortDto = _mapper.Map<SimplecohortDto>(simplecohortEntity);

            return simplecohortDto;
        }

        public async Task<UpdateSimplecohortDto> GetUpdateSimpleCohortDto(Guid id, CancellationToken ct)
        {
            var simplecohortEntity = await _uow.Simplecohorts.GetAsync(id, ct);
            var updateSimplecohortDto = _mapper.Map<UpdateSimplecohortDto>(simplecohortEntity);
            return updateSimplecohortDto;
        }

        public async Task<SimplecohortDto> GetSimplecohortByIdAsync(Guid id, CancellationToken ct)
        {
            SimplecohortDto simplecohortDto = null;

            var simplecohortEntity = await _uow.Simplecohorts.GetAsync(id, ct);
            if (simplecohortEntity != null)
            {
                simplecohortDto = _mapper.Map<SimplecohortDto>(simplecohortEntity);
            }

            return simplecohortDto;
        }

        public async Task DeleteSimplecohortByIdAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var simplecohortEntity = await _uow.Simplecohorts.FirstAsync(c => c.Id == id, ct);
                await _uow.Simplecohorts.DeleteAsync(simplecohortEntity, ct);
                await _uow.SaveChangesAsync(ct);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, string.Empty);
            }
        }

        public async Task<PagedList<SimplecohortDto>> GetSimplecohortsAsync(SimplecohortResourceParameters resourceParameters, CancellationToken ct)
        {
            var simplecohortEntities = _uow.Simplecohorts.GetAll();

            if (!string.IsNullOrEmpty(resourceParameters.Status))
            {
                if (Enum.TryParse(resourceParameters.Status, true, out SimplecohortStatus status))
                {
                    simplecohortEntities = simplecohortEntities.Where(a => a.Status == status);
                }
            }

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = resourceParameters.SearchQuery.Trim().ToLowerInvariant();
                simplecohortEntities = simplecohortEntities
                    .Where(a => a.registrationRequirements.Contains(searchQueryForWhereClause)
                            || a.graduationRequirements.Contains(searchQueryForWhereClause)
                            || a.transferRequirements.Contains(searchQueryForWhereClause));
            }

            var pagedListEntities = await PagedList<Simplecohort>.CreateAsync(simplecohortEntities,
                resourceParameters.PageNumber, resourceParameters.PageSize, ct);

            var result = _mapper.Map<PagedList<SimplecohortDto>>(pagedListEntities);
            result.TotalCount = pagedListEntities.TotalCount;
            result.TotalPages = pagedListEntities.TotalPages;
            result.CurrentPage = pagedListEntities.CurrentPage;
            result.PageSize = pagedListEntities.PageSize;

            return result;
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        {
            if (await _uow.Simplecohorts.GetAsync(id, ct) != null)
                return true;
            
            return false;
        }
    }
}
