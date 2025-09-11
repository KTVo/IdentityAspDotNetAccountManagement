using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet9EFAPI.MVCS.Models.Identity;
using DotNet9EFAPI.MVCS.Services.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers;

[ApiController]
[Route("api/v1/test/user")]
[AllowAnonymous]
public class IdentityUserAuthentication : ControllerBase
{
    private readonly ITestService _testService;
    private readonly UserManager<User> _userManager;

    public IdentityUserAuthentication(ITestService testService, UserManager<User> userManager)
    {
        _testService = testService ?? throw new ArgumentNullException(nameof(testService));
        _userManager = userManager  ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost]
    [Route("register/user")]
    public async Task<IActionResult> SignUpUser([FromBody] User user)
    {
        if (user.PasswordHash == null) { return BadRequest("User password is null"); }
        await _userManager.CreateAsync(user, user.PasswordHash);
        return Ok(true);
    }
}
    

