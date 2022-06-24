using Godot;
using System;

public class Hurtbox : Area2D
{
    [Export]
    private Vector2 HitEffectOffset = Vector2.Zero;

    [Export]
    private bool ShowHitEffect = true;

    private PackedScene hitEffect = (PackedScene)ResourceLoader.Load("res://Misc/HitEffect.tscn");

    public override void _Ready()
    {
        
    }

    public void _on_Hurtbox_area_entered(Hitbox area)
    {     
        var animatedSprite = (Effect)this.hitEffect.Instance();
        animatedSprite.GlobalPosition = this.GlobalPosition + HitEffectOffset;
        this.GetTree().CurrentScene.AddChild(animatedSprite);
    }
}
