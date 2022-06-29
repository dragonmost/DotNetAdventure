using Godot;
using System;

public class Grass : Node2D
{
    private PackedScene grassEffect = (PackedScene)ResourceLoader.Load("res://Misc/GrassEffect.tscn");

    public override void _Ready()
    {        
    }

    public void _on_Hurtbox_area_entered(Area area)
    {
        this.QueueFree();

        var animatedSprite = (Effect)this.grassEffect.Instance();
        animatedSprite.Position = this.Position;
        this.GetParent().AddChild(animatedSprite);
    }
}
