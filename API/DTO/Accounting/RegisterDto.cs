using System.ComponentModel.DataAnnotations;

namespace API.DTO.Accounting;

/// <summary>
/// Represents the data transfer object for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    [EmailAddress]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    [StringLength(12, MinimumLength = 2)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the Id of Province.
    /// </summary>
    [Required]
    public Guid ProvinceId { get; set; }
}
