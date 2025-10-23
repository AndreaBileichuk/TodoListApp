using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Configurations;

public class TodoListMemberConfiguration : IEntityTypeConfiguration<TodoListMember>
{
    public void Configure(EntityTypeBuilder<TodoListMember> builder)
    {
        builder.ToTable("todo_lists_members");

        builder.HasKey(tm => new { tm.UserId, tm.TodoListId });

        builder.HasOne(tm => tm.User)
            .WithMany(u => u.TodoListMemberships)
            .HasForeignKey(tm => tm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(m => m.UserId)
            .HasColumnName("u_id");

        builder.HasOne(tm => tm.TodoList)
            .WithMany(tl => tl.Members)
            .HasForeignKey(tm => tm.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.TodoListId)
            .HasColumnName("td_id");

        builder.Property(m => m.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasColumnName("tlm_role");
    }
}
