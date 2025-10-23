using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.TodoListCommentDtos;

public class AddCommentRequestDto
{
    [Required(ErrorMessage = "Comment text cannot be empty.")]
    [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string Text { get; set; } = string.Empty;
}
