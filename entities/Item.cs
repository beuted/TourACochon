using Godot;
using System;

public class Item : Node2D
{
    ItemType Type = ItemType.CochonSale;

    private Sprite _sprite;

    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");

        _sprite.Frame = (int)Type;
    }

}
