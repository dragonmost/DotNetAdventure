using Godot;
using System;

public class Stats : Node
{
    [Signal]
    delegate void ZeroHp(); 

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
                this.EmitSignal("ZeroHp");
            }
        }
    }

    public override void _Ready()
    {
        this.HP = this.MaxHP;
    }
}
