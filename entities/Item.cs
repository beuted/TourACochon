using Godot;
using System;

public class Item : Node2D
{
    public PigPerks Perks = PigPerks.None;

    private AnimatedSprite _sprite;

    public Vector2 Destination;
    public Vector2 Direction = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");

        _sprite.Frame = (int)Perks;

        Destination = Position;
    }

    public override void _Process(float delta)
    {
        Position += delta * (Destination - Position) * 10f; //TODO: We should move at 32 pixel per sec
    }
}
