using Godot;
using System;

public class Effect : AnimatedSprite
{
    public string AnimationName = "default";

    public override void _Ready()
    {
        this.Connect("animation_finished", this, "OnAnimationFinished");

        this.Visible = true;
        this.Play(this.AnimationName);
    }

    public void OnAnimationFinished()
    {
        QueueFree();
    }
}