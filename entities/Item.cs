using Godot;
using System;

public class Item : Node2D
{
    public PigPerks Perks = PigPerks.None;
    public Vector2 Destination;
    public Vector2 Direction = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        switch (Perks)
        {
            case PigPerks.None:
                GetNode<AnimatedSprite>("Cochon_Dirty").Visible = true;
                break;
            case PigPerks.Cleaned:
                GetNode<AnimatedSprite>("Cochon_Clean").Visible = true;
                break;
            case PigPerks.Fat:
                GetNode<AnimatedSprite>("Cochon_Fat").Visible = true;
                break;
            default:
                GetNode<AnimatedSprite>("Cochon_Clean").Visible = true;
                break;
        }
        Destination = Position;
    }   

    public override void _Process(float delta)
    {
        Position += delta * (Destination - Position) * 10f; //TODO: We should move at 32 pixel per sec
    }
}
