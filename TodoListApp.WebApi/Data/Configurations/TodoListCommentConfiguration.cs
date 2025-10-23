using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data.Configurations;

public class TodoListCommentConfiguration : IEntityTypeConfiguration<TodoListComment>
{
    public void Configure(EntityTypeBuilder<TodoListComment> builder)
    {
        builder.ToTable("todo_list_comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("tlc_id");

        builder.Property(c => c.Text)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnName("tlc_text");

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName("tlc_created_at");

        builder.Property(c => c.UserId)
            .HasColumnName("u_id");

        builder.Property(c => c.TodoListId)
            .HasColumnName("tl_id");

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.TodoList)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
