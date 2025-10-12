using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.ToTable("todo_lists");

        builder.HasKey(tl => tl.Id);

        builder.Property(t => t.Id)
            .HasColumnName("td_id");

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("td_title");

        builder.Property(t => t.Description)
            .IsRequired(false)
            .HasMaxLength(1000)
            .HasColumnName("td_description");
    }
}
