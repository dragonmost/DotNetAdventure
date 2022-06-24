using Godot;
using System;

public class Bat : KinematicBody2D
{
    private const int FRICTION = 600;
    private const int KNOCKBACK = 200;

    private Resource deathEffect;

    private Stats stats;
    private Vector2 knockbackVector = Vector2.Zero;
    
    public override void _Ready()
    {
        this.stats = this.GetNode<Stats>(new NodePath("Stats"));

        this.deathEffect = ResourceLoader.Load("res://Misc/DeathEffect.tscn");
    }

    public override void _PhysicsProcess(float delta)
    {
        knockbackVector = knockbackVector.MoveToward(Vector2.Zero, FRICTION * delta);
        knockbackVector = MoveAndSlide(knockbackVector);
    }

    public void _on_Hurtbox_area_entered(Hitbox area)
    {
        this.stats.HP -= area?.Damage ?? 1;

        var kb = (this.GlobalPosition - area.GetParent<Position2D>().GlobalPosition).Normalized();
        this.knockbackVector = kb * KNOCKBACK;
    }

    public void _on_Stats_ZeroHp()
    {
        QueueFree();

        var animatedSprite = (Effect)((PackedScene)this.deathEffect).Instance();
        animatedSprite.GlobalPosition = this.GlobalPosition;
        this.GetParent().AddChild(animatedSprite);
    }
}
