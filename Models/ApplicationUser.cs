using Microsoft.AspNetCore.Identity;

namespace StriveSteady.Models;

public class ApplicationUser : IdentityUser
{
    public int Id { get; set; }
    public string? Title { get; set; }
}
