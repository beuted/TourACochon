using Godot;
using System;

public class Machine : Node2D
{
    public TileType Type;
    public ItemType? Generate;

    private Node2D _treadmill;
    private Node2D _jonction;
    private Node2D _input;

    public override void _Ready()
    {
        _treadmill = GetNode<Node2D>("Treadmill");
        _jonction = GetNode<Node2D>("Jonction");
        _input = GetNode<Node2D>("Input");
    }

    public Machine Init(TileType tileType)
    {
        _treadmill.Visible = false;
        _jonction.Visible = false;
        _input.Visible = false;

        Type = tileType;

        Generate = tileType.ItemGenerated();

        if (tileType == TileType.TreadmillUp || tileType == TileType.TreadmillRight || tileType == TileType.TreadmillDown || tileType == TileType.TreadmillLeft)
        {
            _treadmill.Visible = true;
            switch (tileType)
            {
                case TileType.TreadmillUp: _treadmill.RotationDegrees = 90; break;
                case TileType.TreadmillRight: _treadmill.RotationDegrees = 0; break;
                case TileType.TreadmillDown: _treadmill.RotationDegrees = 270; break;
                case TileType.TreadmillLeft: _treadmill.RotationDegrees = 180; break;
            }
        }
        else if (tileType == TileType.Jonction)
        {
            _jonction.Visible = true;
        }
        else if (tileType == TileType.InputUp || tileType == TileType.InputRight || tileType == TileType.InputDown || tileType == TileType.InputLeft)
        {
            _input.Visible = true;

            switch (tileType)
            {
                case TileType.TreadmillUp: _input.RotationDegrees = 90; break;
                case TileType.TreadmillRight: _input.RotationDegrees = 0; break;
                case TileType.TreadmillDown: _input.RotationDegrees = 270; break;
                case TileType.TreadmillLeft: _input.RotationDegrees = 180; break;
            }
        }

        return this;
    }

}
