using GearBox.Core.Model.GameObjects.Enemies.Ai;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyCharacter : Character
{
    public EnemyCharacter(string name, int level, Color? color = null) : base(name, level, color)
    {
        AiBehavior = new WanderAiBehavior(this);
    }

    public IAiBehavior AiBehavior { get; set; }

    public override void Update()
    {
        AiBehavior.Update();
        base.Update();
    }
}