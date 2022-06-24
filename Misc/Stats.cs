using Godot;
using System;

public class Stats : Node
{
    [Signal]
    public delegate void ZeroHp();
    public event ZeroHp OnZeroHp;

    [Export]
    private int MaxHP = 1;
    
    private int hp;

    public int HP
    {
        get { return this.hp; }
        set
        {
            hp = value;

            if (this.hp <= 0)
            {
                // triggers both signal and C# event
                this.EmitSignal("ZeroHp");
                OnZeroHp?.Invoke();
            }
        }
    }

    public static int Test{get;set;}

    public override void _Ready()
    {
        this.HP = this.MaxHP;
    }
}
