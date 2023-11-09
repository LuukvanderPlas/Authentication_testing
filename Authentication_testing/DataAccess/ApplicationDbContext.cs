using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication_testing.DataAccess;

public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

    }
}

