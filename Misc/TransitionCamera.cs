using Godot;
using System;

public class TransitionCamera : Camera2D
{
    Vector2 screenSize;
    Vector2 currentScreen = new Vector2(0, 0);
    Player parentPlayer;

    public override void _Ready()
    {
        this.screenSize = new Vector2((int)ProjectSettings.GetSetting("display/window/size/width"), (int)ProjectSettings.GetSetting("display/window/size/height"));
        this.parentPlayer = this.GetParent<Player>();

        this.SetAsToplevel(true);
        this.GlobalPosition = this.parentPlayer.GlobalPosition;
        this.UpdateScreen(this.currentScreen);
    }

    public override void _PhysicsProcess(float delta)
    {
        var parentScreen = new Vector2(this.parentPlayer.GlobalPosition / this.screenSize).Floor();
        if (!parentScreen.IsEqualApprox(this.currentScreen))
        {
            this.UpdateScreen(parentScreen);
        }
    }

    private void UpdateScreen(Vector2 newScreen)
    {
        this.currentScreen = newScreen;
        this.GlobalPosition = this.currentScreen * this.screenSize + (this.screenSize * 0.5f);
    }
}
