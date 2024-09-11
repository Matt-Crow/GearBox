namespace GearBox.Core.Model.Json.AreaUpdate;

/// <summary>
/// A shop a player has open
/// </summary>
public readonly struct OpenShopJson : IChange
{
    public OpenShopJson(
        Guid id,
        string name,
        List<OpenShopOptionJson> buyOptions,
        List<OpenShopOptionJson> sellOptions,
        List<OpenShopOptionJson> buybackOptions
    )
    {
        Id = id;
        Name = name;
        BuyOptions = buyOptions;
        SellOptions = sellOptions;
        BuybackOptions = buybackOptions;
    }

    public Guid Id { get; init; }

    public string Name { get; init; }

    /// <summary>
    /// Things the player can buy
    /// </summary>
    public List<OpenShopOptionJson> BuyOptions { get; init; }

    /// <summary>
    /// Things the player can sell
    /// </summary>
    public List<OpenShopOptionJson> SellOptions { get; init; }

    /// <summary>
    /// Things the player can buy back after selling
    /// </summary>
    public List<OpenShopOptionJson> BuybackOptions { get; init; }

    public IEnumerable<object?> DynamicValues => [Id, Name, ..GetDynamicValues(BuyOptions), ..GetDynamicValues(SellOptions), ..GetDynamicValues(BuybackOptions)];

    private static IEnumerable<object?> GetDynamicValues(List<OpenShopOptionJson> options)
    {
        var result = new List<object?>();
        foreach (var option in options)
        {
            result.AddRange(option.Item.DynamicValues);
            result.Add(option.BuyPrice);
            result.Add(option.CanAfford);
        }
        return result;
    }
}