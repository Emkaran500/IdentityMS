using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMS.Data;
using IdentityMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMS.Controllers;
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IdentityMsDbContext dbContext;

    public IdentityController(IdentityMsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPost("api/[controller]/[action]")]
    public Task<IActionResult> SignUp(User user)
    {
        try
        {
            this.dbContext.Users.Add(user);
            base.Created();
        }
        catch (Exception ex)
        {
            base.BadRequest(ex.Message);
        }

        base.StatusCode(500);
    }
}