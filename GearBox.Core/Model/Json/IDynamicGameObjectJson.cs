using System.Text.Json.Serialization;

namespace GearBox.Core.Model.Json;

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-7-0
[JsonDerivedType(typeof(CharacterJson), "character")] // bad: essentially seals this class so only people with access to this can add new subtypes
[JsonDerivedType(typeof(ProjectileJson), "projectile")]
public interface IDynamicGameObjectJson : IJson
{

}