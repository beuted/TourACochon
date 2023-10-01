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
        var cochonDirty = GetNode<AnimatedSprite>("Cochon_Dirty");
        var cochonClean = GetNode<AnimatedSprite>("Cochon_Clean");
        var cochonFat = GetNode<AnimatedSprite>("Cochon_Fat");
        var pigFood = GetNode<Sprite>("PigFood");

        cochonDirty.Visible = false;
        cochonClean.Visible = false;
        cochonFat.Visible = false;
        pigFood.Visible = false;

        switch (Perks)
        {
            case PigPerks.None:
                cochonDirty.Visible = true;
                break;
            case PigPerks.Cleaned:
                cochonClean.Visible = true;
                break;
            case PigPerks.Fat:
                cochonFat.Visible = true;
                break;
            case PigPerks.PigFood:
                pigFood.Visible = true;
                break;
            default:
                cochonClean.Visible = true;
                break;
        }
        Destination = Position;
    }

    public override void _Process(float delta)
    {
        Position += delta * (Destination - Position) * 10f; //TODO: We should move at 32 pixel per sec
    }
}
