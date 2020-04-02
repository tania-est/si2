using Microsoft.AspNetCore.JsonPatch;
using si2.bll.Dtos.Requests.Simplecohort;
using si2.bll.Dtos.Results.Simplecohort;
using si2.bll.Helpers.PagedList;
using si2.bll.Helpers.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace si2.bll.Services
{
    public interface ISimplecohortService : IServiceBase
    {
        Task<SimplecohortDto> CreateSimplecohortAsync(CreateSimplecohortDto createSimplecohortDto, CancellationToken ct);
        Task<SimplecohortDto> UpdateSimplecohortAsync(Guid id, UpdateSimplecohortDto updateSimplecohortDto, CancellationToken ct);
        Task<SimplecohortDto> PartialUpdateSimplecohortAsync(Guid id, UpdateSimplecohortDto patchDoc, CancellationToken ct);
        Task<UpdateSimplecohortDto> GetUpdateSimpleCohortDto(Guid id, CancellationToken ct);
        Task<SimplecohortDto> GetSimplecohortByIdAsync(Guid id, CancellationToken ct);
        Task DeleteSimplecohortByIdAsync(Guid id, CancellationToken ct);
        Task<PagedList<SimplecohortDto>> GetSimplecohortsAsync (SimplecohortResourceParameters pagedResourceParameters, CancellationToken ct);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    }
}
