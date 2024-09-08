using System.ComponentModel.DataAnnotations;
using API.Data.Interfaces;

namespace API.Entities;

/// <summary>
/// Represents the base entity class that provides a common identifier property.
/// </summary>
public abstract class BaseEntity : IIdentifiable<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
}
