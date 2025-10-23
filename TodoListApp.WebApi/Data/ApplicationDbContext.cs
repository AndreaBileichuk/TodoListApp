using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private const string IdentitySchema = "identity";

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<TodoTask> TodoTasks { get; set; }

    public DbSet<TodoListMember> TodoListMembers { get; set; }

    public DbSet<TodoListComment> TodoListComments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Move all Identity tables to "identity" schema
        builder.Entity<User>().ToTable("AspNetUsers", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().ToTable("AspNetRoles", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("AspNetUserRoles", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("AspNetUserClaims", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("AspNetUserLogins", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable("AspNetUserTokens", schema: IdentitySchema);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", schema: IdentitySchema);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
