namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents the base class for all entities in the application.
/// </summary>
/// <remarks>
/// This class provides common properties such as <see cref="Id"/>, <see cref="CreatedAt"/>, and <see cref="LastModifiedAt"/> 
/// that are inherited by all other entities in the system.
/// </remarks>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified.
    /// </summary>
    public DateTime LastModifiedAt { get; set; }
}