using GearBox.Core.Model.Abilities.Actives;
using Xunit;

namespace GearBox.Core.Tests.Model.Abilities.Actives;

public class ActiveAbilityFactoryTester
{
    [Fact]
    public void Add_GivenDuplicateNames_Throws()
    {
        var sut = new ActiveAbilityFactory();
        sut.Add(new BasicAttack());
        Assert.Throws<ArgumentException>(() => sut.Add(new BasicAttack()));
    }

    [Fact]
    public void Make_GivenInvalidName_ReturnsNull()
    {
        var sut = new ActiveAbilityFactory();
        var actual = sut.Make("foo");
        Assert.Null(actual);
    }

    [Fact]
    public void Make_GivenValidName_ReturnsCopy()
    {
        var sut = new ActiveAbilityFactory();
        var active = new BasicAttack();

        sut.Add(active);
        var actual = sut.Make(active.Name);

        Assert.NotNull(actual);
        Assert.False(active == actual); // referential equality should fail
    }
}