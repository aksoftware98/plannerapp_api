using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerApp.Models;
using PlannerApp.Models.V2.DTO;
using PlannerApp.Models.V2.Responses;
using PlannerApp.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class PlansController : ControllerBase
    {

        private readonly IPlansService _plans;

        public PlansController(IPlansService plans)
        {
            _plans = plans;
        }

        #region Get
        [ProducesResponseType(200, Type = typeof(ApiResponse<PagedList<PlanDetail>>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpGet]
        public async Task<IActionResult> Get(string query, int pageNumber, int pageSize)
        {
            var result = await _plans.GetPlansAsync(query, pageNumber, pageSize);

            return Ok(new ApiResponse<PagedList<PlanDetail>>(result, "Plans retrieved successfully"));
        }

        [ProducesResponseType(200, Type = typeof(ApiResponse<PlanDetail>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _plans.GetByIdAsync(id);

            return Ok(new ApiResponse<PlanDetail>(result, "Plan retrieved successfully"));
        }
        #endregion

        #region Create
        [ProducesResponseType(200, Type = typeof(ApiResponse<PlanDetail>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpPost()]
        public async Task<IActionResult> Post([FromForm]PlanDetail model)
        {
            var result = await _plans.CreateAsync(model);

            return Ok(new ApiResponse<PlanDetail>(result, "Plan created successfully"));
        }
        #endregion

        #region Update
        [ProducesResponseType(200, Type = typeof(ApiResponse<PlanDetail>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpPut()]
        public async Task<IActionResult> Put([FromForm] PlanDetail model)
        {
            var result = await _plans.EditAsync(model);

            return Ok(new ApiResponse<PlanDetail>(result, "Plan edited successfully"));
        }
        #endregion

        #region Delete
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _plans.DeleteAsync(id);

            return Ok(new ApiResponse("Plan deleted successfully"));
        }
        #endregion 

    }
}
