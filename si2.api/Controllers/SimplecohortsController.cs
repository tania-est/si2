using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using si2.bll.Dtos.Requests.Simplecohort;
using si2.bll.Dtos.Results.Simplecohort;
using si2.bll.Helpers.ResourceParameters;
using si2.bll.Services;
using si2.common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace si2.api.Controllers
{
    [ApiController]
    [Route("api/simplecohorts")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public class SimplecohortsController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<SimplecohortsController> _logger;
        private readonly ISimplecohortService _simplecohortService;

        public SimplecohortsController(LinkGenerator linkGenerator, ILogger<SimplecohortsController> logger, ISimplecohortService simplecohortService)
        {
            _linkGenerator = linkGenerator;
            _logger = logger;
            _simplecohortService = simplecohortService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SimplecohortDto))]
        public async Task<ActionResult> CreateSimplecohort([FromBody] CreateSimplecohortDto createSimplecohortDto, CancellationToken ct)
        {
            var simplecohortToReturn = await _simplecohortService.CreateSimplecohortAsync(createSimplecohortDto, ct);
            if (simplecohortToReturn == null)
                return BadRequest();

            return CreatedAtRoute("GetSimplecohort", new { id = simplecohortToReturn.Id }, simplecohortToReturn);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteSimplecohort(Guid id, CancellationToken ct)
        {
            await _simplecohortService.DeleteSimplecohortByIdAsync(id, ct);

            return NoContent();
        }

        [HttpGet("{id}", Name = "GetSimplecohort")]
        //TE - Commented the Authorization for testing Postman
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimplecohortDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSimplecohort(Guid id, CancellationToken ct)
        {
            //TE - Commented the below for testing Postman
            /*var simplecohortDto = await _simplecohortService.GetSimplecohortByIdAsync(id, ct);

            if (simplecohortDto == null)
                return NotFound();

            return Ok(simplecohortDto);*/
            return Ok("Reached");
        }

        [HttpGet(Name = "GetSimplecohorts")]
        public async Task<ActionResult> GetSimplecohorts([FromQuery]SimplecohortResourceParameters pagedResourceParameters, CancellationToken ct)
        {
            var simplecohortDtos = await _simplecohortService.GetSimplecohortsAsync(pagedResourceParameters, ct);

            var previousPageLink = simplecohortDtos.HasPrevious ? CreateSimplecohortsResourceUri(pagedResourceParameters, Enums.ResourceUriType.PreviousPage) : null;
            var nextPageLink = simplecohortDtos.HasNext ? CreateSimplecohortsResourceUri(pagedResourceParameters, Enums.ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = simplecohortDtos.TotalCount,
                pageSize = simplecohortDtos.PageSize,
                currentPage = simplecohortDtos.CurrentPage,
                totalPages = simplecohortDtos.TotalPages,
                previousPageLink,
                nextPageLink
            };

            if (simplecohortDtos == null)
                return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(simplecohortDtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimplecohortDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateSimplecohort([FromRoute]Guid id, [FromBody] UpdateSimplecohortDto updateSimplecohortDto, CancellationToken ct)
        {
            if (!await _simplecohortService.ExistsAsync(id, ct))
                return NotFound();

            var simplecohortToReturn = await _simplecohortService.UpdateSimplecohortAsync(id, updateSimplecohortDto, ct);
            if (simplecohortToReturn == null)
                return BadRequest();

            return Ok(simplecohortToReturn);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateSimplecohort([FromRoute]Guid id, [FromBody] JsonPatchDocument<UpdateSimplecohortDto> patchDoc, CancellationToken ct)
        {
            if (!await _simplecohortService.ExistsAsync(id, ct))
                return NotFound();

            var simplecohortToPatch = await _simplecohortService.GetUpdateSimpleCohortDto(id, ct);
            patchDoc.ApplyTo(simplecohortToPatch, ModelState);

            TryValidateModel(simplecohortToPatch);

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var simplecohortToReturn = await _simplecohortService.PartialUpdateSimplecohortAsync(id, simplecohortToPatch, ct);
            if (simplecohortToReturn == null)
                return BadRequest();

            return Ok(simplecohortToReturn);
        }


        private string CreateSimplecohortsResourceUri(SimplecohortResourceParameters pagedResourceParameters, Enums.ResourceUriType type)
        {
            switch (type)
            {
                case Enums.ResourceUriType.PreviousPage:
                    return _linkGenerator.GetUriByName(this.HttpContext, "GetSimplecohorts",
                        new
                        {
                            status = pagedResourceParameters.Status,
                            searchQuery = pagedResourceParameters.SearchQuery,
                            pageNumber = pagedResourceParameters.PageNumber - 1,
                            pageSize = pagedResourceParameters.PageSize
                        }); // TODO get the aboslute path 
                case Enums.ResourceUriType.NextPage:
                    return _linkGenerator.GetUriByName(this.HttpContext, "GetSimplecohorts",
                        new
                        {
                            status = pagedResourceParameters.Status,
                            searchQuery = pagedResourceParameters.SearchQuery,
                            pageNumber = pagedResourceParameters.PageNumber + 1,
                            pageSize = pagedResourceParameters.PageSize
                        });
                default:
                    return _linkGenerator.GetUriByName(this.HttpContext, "GetSimplecohorts",
                       new
                       {
                           status = pagedResourceParameters.Status,
                           searchQuery = pagedResourceParameters.SearchQuery,
                           pageNumber = pagedResourceParameters.PageNumber,
                           pageSize = pagedResourceParameters.PageSize
                       });
            }
        }
    }
}
