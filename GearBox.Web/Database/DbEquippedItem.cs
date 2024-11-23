using Microsoft.EntityFrameworkCore;

namespace GearBox.Web.Database;

[Owned]
public class DbEquippedItem
{
    public required string Name { get; set; }
    public required int Level { get; set; }
}