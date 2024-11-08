using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_app.Models;

public abstract class BaseModel
{
    [DisplayName("Display Order")]
    [Range(1, int.MaxValue, ErrorMessage = "Set valid value for Display Order")]
    public int DisplayOrder { get; set; }
}