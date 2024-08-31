using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyCharacter : Character
{
    private readonly LootTable _loot;

    public EnemyCharacter(string name, int level = 1, Color? color = null, LootTable? loot = null) : base(name, level, color)
    {
        AiBehavior = new WanderAiBehavior(this);
        _loot = loot ?? new LootTable([]);
        Killed += HandleKilled;
    }

    private void HandleKilled(object? sender, KilledEventArgs e)
    {
        if (e.AttackEvent.AttackUsed.UsedBy is PlayerCharacter player)
        {
            var loot = _loot.GetRandomLoot();
            player.Inventory.Add(loot);
            player.GainXp(Level * 5);
        }
    }

    public IAiBehavior AiBehavior { get; set; }

    public override void Update()
    {
        AiBehavior.Update();
        base.Update();
    }
}