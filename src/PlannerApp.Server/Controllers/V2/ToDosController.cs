﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerApp.Models;
using PlannerApp.Models.V2.DTO;
using PlannerApp.Models.V2.Responses;
using PlannerApp.Server.Interfaces;
using System.Threading.Tasks;

namespace PlannerApp.Server.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDosController : ControllerBase
    {

        private readonly IToDosService _todos;

        public ToDosController(IToDosService todos)
        {
            _todos = todos;
        }

        #region Get
        [ProducesResponseType(200, Type = typeof(ApiResponse<PagedList<ToDoItemDetail>>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpGet("pendings")]
        public async Task<IActionResult> GetPendings(int page, int pageNumber)
        {
            var result = await _todos.GetNotdoneAsync(page, pageNumber);

            return Ok(new ApiResponse<PagedList<ToDoItemDetail>>(result, "Pending ToDos retrieved successfully"));
        }
        #endregion

        #region Create
        [ProducesResponseType(200, Type = typeof(ApiResponse<ToDoItemDetail>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ToDoItemDetail model)
        {
            var result = await _todos.CreateAsync(model);

            return Ok(new ApiResponse<ToDoItemDetail>(result, "ToDo created successfully"));
        }
        #endregion

        #region Update
        [ProducesResponseType(200, Type = typeof(ApiResponse<ToDoItemDetail>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] ToDoItemDetail model)
        {
            var result = await _todos.EditAsync(model);

            return Ok(new ApiResponse<ToDoItemDetail>(result, "ToDo edited successfully"));
        }

        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpPut("Toggle/{id}")]
        public async Task<IActionResult> Toggle(string id)
        {
            await _todos.ToggleItemAsync(id);

            return Ok(new ApiResponse("ToDo toggled successfully"));
        }
        #endregion

        #region Delete
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _todos.DeleteAsync(id);

            return Ok(new ApiResponse("ToDo deleted successfully"));
        }
        #endregion 

    }
}
