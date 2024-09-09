namespace API.DTO.Accounting;

/// <summary>
/// Represents a user data transfer object.
/// </summary>
public class UserDto : BaseDto
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the authentication token of the user.
    /// </summary>
    public string Token { get; set; }
}
