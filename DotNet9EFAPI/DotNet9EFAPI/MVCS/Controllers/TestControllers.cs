using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet9EFAPI.MVCS.Models.DB;
using DotNet9EFAPI.MVCS.Services.Test;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers
{
    [ApiController]
    [Route("api/v1/test")]
    [AllowAnonymous]
    public class TestControllers(ITestService testService) : ControllerBase
    {
        private readonly ITestService _testService = testService ?? throw new ArgumentNullException(nameof(testService));

        [HttpPost]
        [Route("get/all/items")]
        public async Task<IActionResult> GetItemData()
        {

            List<Item>? respItemsList = await _testService.GetItemsListAsync();

            if (respItemsList == null) { return BadRequest(AppMessages.QueryAllItemsFailed); }

            return Ok(respItemsList);
        }
    }
}