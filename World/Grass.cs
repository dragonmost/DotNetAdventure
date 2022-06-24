using Godot;
using System;

public class Grass : Node2D
{
    private Resource grassEffect;

    public override void _Ready()
    {        
        this.grassEffect = ResourceLoader.Load("res://Misc/GrassEffect.tscn");
    }

    public void _on_Hurtbox_area_entered(Area area)
    {
        this.QueueFree();

        var animatedSprite = (Effect)((PackedScene)this.grassEffect).Instance();
        animatedSprite.GlobalPosition = this.GlobalPosition;
        this.GetParent().AddChild(animatedSprite);
    }
}
