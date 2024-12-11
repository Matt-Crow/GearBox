using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Model.Items;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyCharacter : Character
{
    public EnemyCharacter(string name, int level = 1, Color? color = null, LootTable? loot = null) : base(name, level, color)
    {
        AiBehavior = new WanderAiBehavior(this);
        Loot = loot ?? new LootTable([]);
    }

    public IAiBehavior AiBehavior { get; set; }
    public LootTable Loot { get; init; }

    public override void Update()
    {
        AiBehavior.Update();
        base.Update();
    }
}