using Godot;
using System;

public class Stats : Node
{
    [Signal]
    public delegate void ZeroHp();
    public event ZeroHp OnZeroHp;

    [Signal]
    public delegate void HpChanged();
    public event HpChanged OnHpChanged;

    [Signal]
    public delegate void MaxHpChanged();
    public event MaxHpChanged OnMaxHpChanged;

    [Export]
    private int maxHP = 1;

    public int MaxHP
    {
        get { return this.maxHP; }
        set
        {
            maxHP = value;

            this.EmitSignal("MaxHpChanged");
            OnMaxHpChanged?.Invoke();
        }
    }
    
    private int hp;

    public int HP
    {
        get { return this.hp; }
        set
        {
            hp = value > MaxHP ? MaxHP : value;

            this.EmitSignal("HpChanged");
            OnHpChanged?.Invoke();

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
