using System.ComponentModel.DataAnnotations;

namespace Rocky_app.Models;

public sealed class ApplicationType : BaseModel
{
    [Key]
    public int Id { get; set; }
    
    public string ApplicationTypeName { get; set; } = string.Empty;
    
}