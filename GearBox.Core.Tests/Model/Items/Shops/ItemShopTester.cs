using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Shops;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Shops;

public class ItemShopTester
{
    [Fact]
    public void BuyFrom_RemovesFromInventory()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop();

        sut.BuyFrom(player, item);

        Assert.Empty(player.Inventory.Materials.Content);
    }

    [Fact]
    public void BuyFrom_AddsToBuybackListForPlayer()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop();

        sut.BuyFrom(player, item);

        Assert.True(sut.GetBuybackOptionsFor(player).Contains(item), "Buyback options should contain item");
    }

    [Fact]
    public void BuyFrom_DoesNotAddToBuybackForOtherPlayers()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop();

        sut.BuyFrom(player, item);

        Assert.Empty(sut.GetBuybackOptionsFor(MakePlayer()).Materials.Content);
    }

    [Fact]
    public void BuyFrom_GivesGold()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop();

        sut.BuyFrom(player, item);

        Assert.True(player.Inventory.Gold.Quantity > 0, "Player gold should be positive after selling");
    }

    [Fact]
    public void SellTo_GivenInStockAndSufficientFunds_AddsToInventory()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop(item);

        sut.SellTo(player, item);

        AssertInventoryContains(item, player);
    }

    [Fact]
    public void SellTo_HasInfiniteStock()
    {
        var item = ItemUnion.Of(new Material(new ItemType("foo")));
        var player = MakePlayer();
        player.Inventory.Add(new Gold(item.BuyValue().Quantity * 10));
        var sut = MakeItemShop(item);

        sut.SellTo(player, item);
        sut.SellTo(player, item);
        sut.SellTo(player, item);

        Assert.Equal(3, player.Inventory.Materials.Content.Sum(s => s.Quantity));
    }

    [Fact]
    public void SellTo_GivenInStockAndSufficientFunds_RemovesGold()
    {
        var player = MakePlayer();
        var item = MakeItem();
        player.Inventory.Add(item.BuyValue());
        var sut = MakeItemShop(item);

        sut.SellTo(player, item);

        Assert.Equal(0, player.Inventory.Gold.Quantity);
    }

    [Fact]
    public void SellTo_GivenNotInStock_DoesNotAddToInventory()
    {
        var player = MakePlayer();
        var sut = MakeItemShop();

        sut.SellTo(player, MakeItem());

        Assert.Empty(player.Inventory.Materials.Content);
    }

    [Fact]
    public void SellTo_GivenInsufficientFunds_DoesNotAddToInventory()
    {
        var player = MakePlayer();
        var sut = MakeItemShop();

        sut.SellTo(player, MakeItem());

        Assert.Empty(player.Inventory.Materials.Content);
    }

    [Fact]
    public void SellTo_GivenNotInStockButInBuyback_AddsToInventory()
    {
        var item = MakeItem();
        var player = MakePlayer(item);
        var sut = MakeItemShop();

        sut.BuyFrom(player, item);
        sut.SellTo(player, item);

        AssertInventoryContains(item, player);
    }

    [Fact]
    public void CannotBuybackMultipleTimes()
    {
        var item = ItemUnion.Of(new Material(new ItemType("foo material")));
        var player = MakePlayer(item);
        player.Inventory.Add(new Gold(item.BuyValue().Quantity * 2));
        var sut = MakeItemShop();
        sut.BuyFrom(player, item);

        sut.SellTo(player, item);
        sut.SellTo(player, item);

        Assert.Equal(1, player.Inventory.Materials.Content.Sum(s => s.Quantity));
    }

    [Fact]
    public void SellTo_GivenInStockAndBuyback_TakesFromBuybackFirst()
    {
        var item = ItemUnion.Of(new Material(new ItemType("foo material")));
        var player = MakePlayer(item);
        var sut = MakeItemShop(item);

        sut.BuyFrom(player, item);
        sut.SellTo(player, item);

        Assert.Empty(sut.GetBuybackOptionsFor(player).Materials.Content);
    }

    private static PlayerCharacter MakePlayer(ItemUnion? item = null)
    {
        var result = new PlayerCharacter("a player");
        result.Inventory.Add(item);
        return result;
    }

    private static ItemShop MakeItemShop(ItemUnion? item = null)
    {
        var stock = new Inventory();
        stock.Add(item);
        var result = new ItemShop(stock);
        return result;
    }

    private static ItemUnion MakeItem(string? name = null) => ItemUnion.Of(new Material(new ItemType(name ?? "a material")));

    private static void AssertInventoryContains(ItemUnion item, PlayerCharacter player)
    {
        Assert.True(player.Inventory.Contains(item), "item not found in inventory");
    }
}