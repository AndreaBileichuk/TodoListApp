using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoListApp.WebApp.Enums;

namespace TodoListApp.WebApp.Models.TodoTask;

public class UpdateTaskStatusDto
{
    [Required]
    [EnumDataType(typeof(TodoTaskStatus))]
    [JsonPropertyName("todoTaskStatus")]
    public TodoTaskStatus TodoTaskStatus { get; set; }
}
