using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerApp.Models.V2.DTO;
using PlannerApp.Models.V2.Responses;
using PlannerApp.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {

        private readonly IPlansService _plans;

        public PlansController(IPlansService plans)
        {
            _plans = plans;
        }

        #region Get
        [ProducesResponseType(200, Type = typeof(ApiResponse<PagedList<PlanDetail>>))]
        [ProducesResponseType(400)]
        [HttpGet]
        public async Task<IActionResult> Get(string query, int page, int pageNumber)
        {
            var result = await _plans.GetPlansAsync(query, page, pageNumber);
        }
        #endregion

        #region Create

        #endregion

        #region Update

        #endregion

        #region Delete

        #endregion 

    }
}
