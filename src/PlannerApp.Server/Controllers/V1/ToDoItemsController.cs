using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerApp.Models;
using PlannerApp.Server.Services;

namespace PlannerApp.Server.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoItemsController : ControllerBase
    {

        private readonly IItemsService _itemsService;

        public ToDoItemsController(IItemsService itemsSerivce)
        {
            _itemsService = itemsSerivce;   
        }

        #region GET
        [ProducesResponseType(200, Type = typeof(CollectionResponse<ToDoItem>))]
        [HttpGet("plan={planId}")]
        public IActionResult Get(string plan)
        {
            if (plan == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var items = _itemsService.GetAllItems(plan, userId);
            return Ok(new CollectionResponse<ToDoItem>
            {
                Count = items.Count(),
                IsSuccess = true,
                Message = "Items retrieved successfully!",
                Records = items
            });
        }

        // Get not acheived items 
        [ProducesResponseType(200, Type = typeof(CollectionResponse<ToDoItem>))]
        [HttpGet("notachieved")]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var items = _itemsService.GetNotAchievedItems(userId);
            return Ok(new CollectionResponse<ToDoItem>
            {
                Count = items.Count(),
                IsSuccess = true,
                Message = "Items retrieved successfully!",
                Records = items
            });
        }

        #endregion

        #region POST
        [ProducesResponseType(200, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ToDoItem>))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ToDoItemRequest model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var plan = await _itemsService.CreateItemAsync(model.PlanId, model.Description, model.EstimatedDate, userId);

                return Ok(new OperationResponse<ToDoItem>
                {
                    IsSuccess = true,
                    Message = "Item has been inserted successfully",
                    Record = plan
                });
            }

            return BadRequest(new OperationResponse<ToDoItem>
            {
                IsSuccess = true,
                Message = "Some properties are not valid"
            });
        }
        #endregion

        #region PUT
        [ProducesResponseType(200, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(404)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ToDoItemRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                var plan = await _itemsService.EditItemsAsync(model.Id, model.Description, model.EstimatedDate, userId);
                if (plan == null)
                    return NotFound();

                return Ok(new OperationResponse<ToDoItem>
                {
                    IsSuccess = true,
                    Message = "Item has been edited successfully",
                    Record = plan
                });
            }

            return BadRequest(new OperationResponse<ToDoItem>
            {
                IsSuccess = true,
                Message = "Some properties are not valid"
            });
        }

        /// <summary>
        /// Change the status of the item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var plan = await _itemsService.MarkItemAsync(id, userId);
            if (plan == null)
                return NotFound();

            return Ok(new OperationResponse<ToDoItem>
            {
                IsSuccess = true,
                Message = "Item status changed successfully! successfully!",
                Record = plan
            });
        }
        #endregion

        #region DELETE
        [ProducesResponseType(200, Type = typeof(OperationResponse<ToDoItem>))]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var plan = await _itemsService.DeleteItemAsync(id, userId);
            if (plan == null)
                return NotFound();

            return Ok(new OperationResponse<ToDoItem>
            {
                IsSuccess = true,
                Message = "Item deleted successfully!",
                Record = plan
            });
        }
        #endregion

    }
}