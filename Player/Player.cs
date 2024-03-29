using Godot;
using System;

public class Player : KinematicBody2D
{
    private const int ACCELERATION = 500;
    private const int FRICTION = 700;
    private const int MAX_SPEED = 90;
    private const int ROLL_SPEED = (int)(MAX_SPEED * 1.5);

    private PlayerStats stats;

    private AnimationPlayer animationPlayer;
    private AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback animationState;

    private State state;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 rollVector = Vector2.Down;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        stats = GetNode<PlayerStats>("/root/PlayerStats");
        
        InitializeEvents();

        this.animationPlayer = this.GetNode<AnimationPlayer>(new NodePath("AnimationPlayer"));
        this.animationTree = this.GetNode<AnimationTree>(new NodePath("AnimationTree"));
        this.animationState = (AnimationNodeStateMachinePlayback)this.animationTree.Get("parameters/playback");

        this.animationTree.Active = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        switch(this.state)
        {
            case State.attack:
                this.AttackState(delta);
            break;
            case State.roll:
                this.RollState(delta);
            break;
            case State.move:
                this.MoveState(delta);
            break; 
        }
    }

    private void AttackState(float delta)
    {
        this.velocity = Vector2.Zero;
        this.animationState.Travel("Attack");
    }

    private void RollState(float delta)
    {
        this.velocity = rollVector * ROLL_SPEED;
        this.animationState.Travel("Roll");
        MoveAndSlide(velocity);
        velocity = velocity * 0.25f;    // reduces end of roll slide
    }

    public void OnAnimationFinished()
    {
        this.state = State.move;
    }

    private void MoveState(float delta)
    {
        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        inputVector = inputVector.Normalized();

        if (inputVector != Vector2.Zero)
        {
            this.animationTree.Set("parameters/Idle/blend_position", inputVector);
            this.animationTree.Set("parameters/Run/blend_position", inputVector);
            this.animationTree.Set("parameters/Roll/blend_position", inputVector);
            this.animationTree.Set("parameters/Attack/blend_position", inputVector);

            this.rollVector = inputVector;

            this.animationState.Travel("Run");

            velocity = velocity.MoveToward(inputVector * MAX_SPEED, ACCELERATION * delta);
        }
        else
        {
            this.animationState.Travel("Idle");

            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
        }

        MoveAndSlide(velocity);

        this.CheckState();
    }

    private void CheckState()
    {
        if(Input.IsActionJustPressed("attack"))
        {
            this.state = State.attack;
        }

        if(Input.IsActionJustPressed("roll"))
        {
            this.state = State.roll;
        }
    }

    private void InitializeEvents()
    {
        this.stats.OnZeroHp += () => GD.Print("DEAD");
    }

    private void _on_Hurtbox_area_entered(Hitbox area)
    {
        this.stats.HP -= area?.Damage ?? 1;
    }

    private enum State
    {
        move,
        roll,
        attack,
    }
}
