using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Units;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class LootChestTester
{
    [Fact]
    public void CheckForCollisions_GivenPlayerCollides_AddsToInventory()
    {
        var item = new Item(new ItemType("foo"));
        var sut = new LootChest(Coordinates.ORIGIN, item);
        var player = new PlayerCharacter(new Character()
        {
            Coordinates = sut.Body.Location
        });

        sut.CheckForCollisions(player);

        Assert.Contains(item, player.Inventory.Materials.Content.Select(stack => stack.Item));
    }
}