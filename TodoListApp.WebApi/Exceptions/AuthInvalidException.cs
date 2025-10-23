using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApi.Exceptions;

public class AuthInvalidException : Exception
{
    public IEnumerable<IdentityError> Errors { get; } = [];

    public AuthInvalidException(){}

    public AuthInvalidException(IEnumerable<IdentityError> errors)
        : base("One or more authentication errors occurred.")
    {
        this.Errors = errors;
    }

    public AuthInvalidException(string message) : base(message)
    {
    }

    public AuthInvalidException(string message, Exception innerException) : base(message, innerException) { }
}
