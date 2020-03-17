using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlannerApp.Models;
using PlannerApp.Server.Services;

namespace PlannerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlansController : ControllerBase
    {

        private readonly IPlansService _plansService;
        private readonly IConfiguration _configuration;

        private const int PAGE_SIZE = 10;
        public PlansController(IPlansService plansService, IConfiguration configuration)
        {
            _plansService = plansService;
            _configuration = configuration;
        }

        private readonly List<string> allowedExtensions = new List<string>
        {
            ".jpg", ".bmp", ".png"
        };

        #region Get
        [ProducesResponseType(200, Type = typeof(CollectionPagingResponse<Plan>))]
        [HttpGet]
        public IActionResult Get(int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int totalPlans = 0;
            if (page == 0)
                page = 1;
            var plans = _plansService.GetAllPlansAsync(PAGE_SIZE, page, userId, out totalPlans);

            int totalPages = 0;
            if (totalPlans % PAGE_SIZE == 0)
                totalPages = totalPlans / PAGE_SIZE;
            else
                totalPages = (totalPlans / PAGE_SIZE) + 1;

            return Ok(new CollectionPagingResponse<Plan>
            {
                Count = plans.Count(),
                IsSuccess = true,
                Message = "Plans received successfully!",
                OperationDate = DateTime.UtcNow,
                PageSize = PAGE_SIZE,
                Page = page,
                Records = plans
            });
        }

        [ProducesResponseType(200, Type = typeof(CollectionPagingResponse<Plan>))]
        [HttpGet("query={query}")]
        public IActionResult Get(string query, int page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int totalPlans = 0;
            if (page == 0)
                page = 1;
            var plans = _plansService.SearchPlansAsync(query, PAGE_SIZE, page, userId, out totalPlans);

            int totalPages = 0;
            if (totalPlans % PAGE_SIZE == 0)
                totalPages = totalPlans / PAGE_SIZE;
            else
                totalPages = (totalPlans / PAGE_SIZE) + 1;

            return Ok(new CollectionPagingResponse<Plan>
            {
                Count = plans.Count(),
                IsSuccess = true,
                Message = $"Plans of '{query}' received successfully!",
                OperationDate = DateTime.UtcNow,
                PageSize = PAGE_SIZE,
                Page = page,
                Records = plans
            });
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<Plan>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Plan>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var plan = await _plansService.GetPlanById(id, userId);
            if (plan == null)
                return BadRequest(new OperationResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid operation",
                });

            return Ok(new OperationResponse<Plan>
            {
                Record = plan,
                Message = "Plan retrieved successfully!",
                IsSuccess = true,
                OperationDate = DateTime.UtcNow
            });
        }

        #endregion

        #region Post 
        [ProducesResponseType(200, Type = typeof(OperationResponse<Plan>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Plan>))]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]PlanRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string url = $"{_configuration["AppUrl"]}Images/default.jpg";
            string fullPath = null;
            // Check the file 
            if (model.CoverFile != null)
            {
                string extension = Path.GetExtension(model.CoverFile.FileName);

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new OperationResponse<Plan>
                    {
                        Message = "Plan image is not a valid image file",
                        IsSuccess = false,
                    });

                if (model.CoverFile.Length > 500000)
                    return BadRequest(new OperationResponse<Plan>
                    {
                        Message = "Image file cannot be more than 5mb",
                        IsSuccess = false,
                    });

                string newFileName = $"Images/{Guid.NewGuid()}{extension}";
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newFileName);
                url = $"{_configuration["AppUrl"]}{newFileName}";
            }

            var addedPlan = await _plansService.AddPlanAsync(model.Title, model.Description, url, userId);

            if(addedPlan != null)
            {
                if(fullPath != null)
                {
                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.CoverFile.CopyToAsync(fs);
                    }
                }

                return Ok(new OperationResponse<Plan>
                {
                    IsSuccess = true,
                    Message = $"{addedPlan.Title} has been added successfully!",
                    Record = addedPlan
                });

            }

            return BadRequest(new OperationResponse<Plan>
            {
                Message = "Something went wrong",
                IsSuccess = false
            });

        }
        #endregion

        #region Put 
        [ProducesResponseType(200, Type = typeof(OperationResponse<Plan>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<Plan>))]
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]PlanRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string url = $"{_configuration["AppUrl"]}Images/default.jpg";
            string fullPath = null;
            // Check the file 
            if (model.CoverFile != null)
            {
                string extension = Path.GetExtension(model.CoverFile.FileName);

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new OperationResponse<Plan>
                    {
                        Message = "Plan image is not a valid image file",
                        IsSuccess = false,
                    });

                if (model.CoverFile.Length > 500000)
                    return BadRequest(new OperationResponse<Plan>
                    {
                        Message = "Image file cannot be more than 5mb",
                        IsSuccess = false,
                    });

                string newFileName = $"Images/{Guid.NewGuid()}{extension}";
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newFileName);
                url = $"{_configuration["AppUrl"]}{newFileName}";
            }
            var oldPlan = await _plansService.GetPlanById(model.Id, userId);
            if (fullPath == null)
                url = oldPlan.CoverPath;

            var editedPlan = await _plansService.EditPlanAsync(model.Id, model.Title, model.Description, url, userId);

            if (editedPlan != null)
            {
                if (fullPath != null)
                {
                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.CoverFile.CopyToAsync(fs);
                    }
                }

                return Ok(new OperationResponse<Plan>
                {
                    IsSuccess = true,
                    Message = $"{editedPlan.Title} has been edited successfully!",
                    Record = editedPlan
                });
            }


            return BadRequest(new OperationResponse<Plan>
            {
                Message = "Something went wrong",
                IsSuccess = false
            });
        
        }
        #endregion

        #region Delete
        [ProducesResponseType(200, Type = typeof(OperationResponse<Plan>))]
        [ProducesResponseType(404)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var getOld = await _plansService.GetPlanById(id, userId);
            if (getOld == null)
                return NotFound();

            // Remove the file 
            //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", getOld.CoverPath.Replace(_configuration["AppUrl"], ""));
            //System.IO.File.Delete(fullPath);

            var deletedPlan = await _plansService.DeletePlanAsync(id, userId);

            return Ok(new OperationResponse<Plan>
            {
                IsSuccess = true,
                Message = $"{getOld.Title} has been deleted successfully!",
                Record = deletedPlan
            });
        }
        #endregion 


    }
}