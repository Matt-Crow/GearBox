using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class LootChestTester
{
    [Fact]
    public void CheckForCollisions_GivenPlayerCollides_AddsToInventory()
    {
        var inventory = new Inventory();
        var item = new Material("foo");
        inventory.Materials.Add(item);
        var sut = new LootChest(Coordinates.ORIGIN, inventory);
        var player = new PlayerCharacter("bar")
        {
            Coordinates = sut.Body.Location
        };

        sut.CheckForCollisions(player);

        Assert.Contains(item, player.Inventory.Materials.Content.Select(stack => stack.Item));
    }
}