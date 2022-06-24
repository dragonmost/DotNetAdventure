using Godot;
using System;

public class Hurtbox : Area2D
{
    [Export]
    private Vector2 HitEffectOffset = Vector2.Zero;

    [Export]
    private float InvicibilityTime = 0.1f;

    [Export]
    private bool ShowHitEffect = true;

    private PackedScene hitEffect = (PackedScene)ResourceLoader.Load("res://Misc/HitEffect.tscn");
    
    private Timer timer;

    public override void _Ready()
    {
        this.timer = this.GetNode<Timer>(new NodePath("Timer"));
    }

    public void _on_Hurtbox_area_entered(Area2D area)
    {
        this.CreateHitEffect();
        this.SetDeferred("monitoring", false);
        this.timer.Start(InvicibilityTime);
    }

    public void _on_Timer_timeout()
    {
        this.Monitoring = true;
    }

    public void CreateHitEffect()
    {     
        if (this.ShowHitEffect)
        {
            var animatedSprite = (Effect)this.hitEffect.Instance();
            animatedSprite.GlobalPosition = this.GlobalPosition + HitEffectOffset;
            this.GetTree().CurrentScene.AddChild(animatedSprite);
        }
    }
}
