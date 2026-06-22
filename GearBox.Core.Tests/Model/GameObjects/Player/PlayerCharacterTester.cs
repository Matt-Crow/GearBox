using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Json.AreaUpdate;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Install_GivenNotInInventory_DoesNothing()
    {
        var sut = new PlayerCharacter("foo");
        sut.InstallById(Guid.Empty);
        Assert.Empty(sut.PartSlots.Where(slot => slot.Part != null));
    }

    [Fact]
    public void Install_GivenPart_SetsAppropriateSlot()
    {
        var sut = new PlayerCharacter("foo");
        var part = APart();
        sut.Inventory.Add(part);

        sut.InstallById(GetId(part));

        Assert.Equal(part, sut.GetSlotFor(part).Part);
    }

    [Fact]
    public void Install_GivenPart_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo");
        var part = APart();
        sut.Inventory.Add(part);

        sut.InstallById(GetId(part));

        Assert.False(sut.Inventory.Contains(ItemUnion.OfPart(part)));
    }

    [Fact]
    public void Install_GivenSlotAlreadyHasPartInstalled_AddsToInventory()
    {
        var sut = new PlayerCharacter("foo");
        var alreadyInstalled = new Part("Part 1", PartSlotType.ALL.First());
        var notYetInstalled = new Part("Part 2", PartSlotType.ALL.First());
        sut.Inventory.Add(alreadyInstalled);
        sut.Inventory.Add(notYetInstalled);
        sut.InstallById(GetId(alreadyInstalled));

        sut.InstallById(GetId(notYetInstalled));

        Assert.True(sut.Inventory.Contains(ItemUnion.OfPart(alreadyInstalled)));
    }

    [Fact]
    public void CannotInstallAboveOwnLevel()
    {
        var sut = new PlayerCharacter("foo");
        sut.SetLevel(1);
        var part = new Part("bar", PartSlotType.ALL.First(), level: 20);
        sut.Inventory.Add(part);

        sut.InstallById(GetId(part));

        Assert.Null(sut.GetSlotFor(part).Part);
    }

    [Fact]
    public void DynamicValues_AfterInstalling_Change()
    {
        var player = new PlayerCharacter("foo");
        var part = APart();
        player.Inventory.Add(part);
        var before = new UiState(player);
        
        player.InstallById(GetId(part));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(SlotHasChanged(compared, part));
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var player = new PlayerCharacter("foo");
        var part1 = APart();
        var part2 = APart(); // same part, different ID
        player.Inventory.Add(part1);
        player.Inventory.Add(part2);
        player.InstallById(GetId(part1));
        var before = new UiState(player);

        player.InstallById(GetId(part2));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(SlotHasChanged(compared, part1));
    }

    private Guid GetId(Part part)
    {
        return part.Id ?? throw new Exception("ID should not be null");
    }

    private bool SlotHasChanged(UiStateChangesJson changes, Part part)
    {
        var slot = changes.PartSlots.First(s => s.SlotType == part.SlotType.Name);
        return slot.Part.HasChanged;
    }

    private Part APart() => new Part("Some part", PartSlotType.ALL.First());
}