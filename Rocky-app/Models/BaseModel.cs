using System.ComponentModel;

namespace Rocky_app.Models;

public abstract class BaseModel
{
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}