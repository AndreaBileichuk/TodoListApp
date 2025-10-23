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

        builder.Property(t => t.Id).HasColumnName("t_id");

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
            .HasColumnName("t_created_at");

        builder.Property(t => t.DueDate)
            .IsRequired()
            .HasColumnName("t_due_date");

        builder.Property(t => t.AssigneeId)
            .IsRequired()
            .HasColumnName("u_assignee_id");

        builder.Property(t => t.CreatedById)
            .IsRequired()
            .HasColumnName("u_creator_id");

        builder.Property(t => t.TodoListId)
            .IsRequired()
            .HasColumnName("td_id");

        builder.HasOne(t => t.TodoList)
            .WithMany(tl => tl.TodoTasks)
            .HasForeignKey(t => t.TodoListId);

        builder.HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
