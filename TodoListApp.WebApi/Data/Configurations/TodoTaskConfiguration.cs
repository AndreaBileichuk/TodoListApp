using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Configurations;

public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("t_id");

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("t_title");

        builder.Property(t => t.Description)
            .IsRequired(false)
            .HasMaxLength(255)
            .HasColumnName("t_description");

        builder.Property(t => t.Status)
            .HasDefaultValue(TodoTaskStatus.NotStarted)
            .HasColumnName("t_task_status");

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName("t_create_at");

        builder.Property(t => t.DueDate)
            .IsRequired()
            .HasColumnName("t_due_date");

        builder.Property(t => t.AssigneeId)
            .IsRequired()
            .HasColumnName("u_id");

        builder.Property(t => t.TodoListId)
            .IsRequired()
            .HasColumnName("td_id");

        builder.HasOne(t => t.TodoList)
            .WithMany(tl => tl.TodoTasks)
            .HasForeignKey(t => t.TodoListId);
    }
}
