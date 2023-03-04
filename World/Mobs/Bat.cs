using Godot;
using System;

public class Bat : KinematicBody2D, IMob
{
    private const int ACCELERATION = 300;
    private const int MAX_SPEED = 50;
    private const int FRICTION = 200;

    private const int KNOCKBACK_FRICTION = 600;
    private const int KNOCKBACK = 200;

    private PackedScene deathEffect = (PackedScene)ResourceLoader.Load("res://Misc/DeathEffect.tscn");

    private Stats stats;
    private AnimatedSprite animatedSprite;
    private SoftCollision softCollision;
    private PlayerDetectionZone playerDetectionZone;

    private State state;

    private Vector2 velocity = Vector2.Zero;
    private Vector2 knockbackVector = Vector2.Zero;
    
    public override void _Ready()
    {
        this.stats = this.GetNode<Stats>(new NodePath("Stats"));
        this.animatedSprite = this.GetNode<AnimatedSprite>(new NodePath("Sprite"));
        this.softCollision = this.GetNode<SoftCollision>(new NodePath("SoftCollision"));
        this.playerDetectionZone = this.GetNode<PlayerDetectionZone>(new NodePath("PlayerDetectionZone"));

        this.animatedSprite.Play();
    }

    public override void _PhysicsProcess(float delta)
    {
        knockbackVector = knockbackVector.MoveToward(Vector2.Zero, KNOCKBACK_FRICTION * delta);
        knockbackVector = MoveAndSlide(knockbackVector);

        switch(this.state)
        {
            case State.Idle:
                velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
                this.state = this.playerDetectionZone.Player != null ? State.Chase : this.state;
            break;
            case State.Wander:
                this.state = this.playerDetectionZone.Player != null ? State.Chase : this.state;
            break;
            case State.Chase:
                if (this.playerDetectionZone.Player != null)
                {
                    this.animatedSprite.FlipH = velocity.x < 0;
                    var direction = (this.playerDetectionZone.Player.GlobalPosition - this.GlobalPosition).Normalized();
                    this.velocity = this.velocity.MoveToward(direction * MAX_SPEED, ACCELERATION * delta);
                }
                else 
                {
                    this.state = State.Idle;
                }
            break;
        }

        velocity += this.softCollision.GetPushVector * delta * 400;
        velocity = MoveAndSlide(this.velocity);
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

        var animatedSprite = (Effect)this.deathEffect.Instance();
        animatedSprite.GlobalPosition = this.Position;
        this.GetParent().AddChild(animatedSprite);
    }

    private enum State
    {
        Idle,
        Wander,
        Chase,
    }
}
