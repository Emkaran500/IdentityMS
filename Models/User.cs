using Microsoft.AspNetCore.Identity;

namespace IdentityMS.Models;

public class User : IdentityUser
{
    public string Secret { get; set; }
}