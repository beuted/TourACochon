using Godot;
using System;
using System.Xml.Serialization;

public class Machine : Node2D
{
    public TileType TileType
    {
        get => _tileType;
        set
        {
            _tileType = value;
            OnTileTypeChanged();
        }
    }
    public bool IsCreatedByUser;

    private Node2D _treadmill;
    private Node2D _jonction;
    private Node2D _input;
    private Node2D _output;
    private Sprite _treadmillSprite;
    private Node2D _washingMaching;
    private Node2D _feedingMachine;
    private Node2D _brick;
    private TileType _tileType;

    public override void _Ready()
    {
        _treadmill = GetNode<Node2D>("Treadmill");
        _washingMaching = GetNode<Node2D>("MachineWasher");
        _feedingMachine = GetNode<Node2D>("MachineFeeder");
        _jonction = GetNode<Node2D>("Jonction");
        _input = GetNode<Node2D>("Input");
        _output = GetNode<Node2D>("Output");
        _brick = GetNode<Node2D>("Brick");

        _treadmillSprite = GetNode<Sprite>("Treadmill/Sprite");

        OnTileTypeChanged();
    }

    public override void _Process(float delta)
    {
        if (TileType == TileType.TreadmillUp || TileType == TileType.TreadmillRight || TileType == TileType.TreadmillDown || TileType == TileType.TreadmillLeft)
        {
            var time = OS.GetSystemTimeMsecs();
            _treadmillSprite.Frame = (int)((time % 800ul) / 100ul); // This is done instead of using an animtation player in order to keep all treadmills in syn visually
        }
    }

    private void OnTileTypeChanged()
    {
        if (_treadmill == null)
        {
            // Not properly initialized yet
            return;
        }

        ResetVisibility();

        if (TileType == TileType.TreadmillUp || TileType == TileType.TreadmillRight || TileType == TileType.TreadmillDown || TileType == TileType.TreadmillLeft)
        {
            _treadmill.Visible = true;
            switch (TileType)
            {
                case TileType.TreadmillUp: _treadmill.RotationDegrees = 270; break;
                case TileType.TreadmillRight: _treadmill.RotationDegrees = 0; break;
                case TileType.TreadmillDown: _treadmill.RotationDegrees = 90; break;
                case TileType.TreadmillLeft: _treadmill.RotationDegrees = 180; break;
            }
        }
        else if (TileType == TileType.Jonction)
        {
            _jonction.Visible = true;
        }
        else if (TileType == TileType.InputUp || TileType == TileType.InputRight || TileType == TileType.InputDown || TileType == TileType.InputLeft)
        {
            _input.Visible = true;

            switch (TileType)
            {
                case TileType.InputUp: _input.RotationDegrees = 270; break;
                case TileType.InputRight: _input.RotationDegrees = 0; break;
                case TileType.InputDown: _input.RotationDegrees = 90; break;
                case TileType.InputLeft: _input.RotationDegrees = 180; break;
            }
        }
        else if (TileType == TileType.OutputRight || TileType == TileType.OutputDown || TileType == TileType.OutputLeft || TileType == TileType.OutputUp)
        {
            _output.Visible = true;

            switch (TileType)
            {
                case TileType.OutputUp: _output.RotationDegrees = 270; break;
                case TileType.OutputRight: _output.RotationDegrees = 0; break;
                case TileType.OutputDown: _output.RotationDegrees = 90; break;
                case TileType.OutputLeft: _output.RotationDegrees = 180; break;
            }
        }
        else if (TileType == TileType.MachineWasherRight || TileType == TileType.MachineWasherDown || TileType == TileType.MachineWasherLeft || TileType == TileType.MachineWasherUp)
        {
            _washingMaching.Visible = true;

            switch (TileType)
            {
                case TileType.MachineWasherUp: _washingMaching.RotationDegrees = 270; break;
                case TileType.MachineWasherRight: _washingMaching.RotationDegrees = 0; break;
                case TileType.MachineWasherDown: _washingMaching.RotationDegrees = 90; break;
                case TileType.MachineWasherLeft: _washingMaching.RotationDegrees = 180; break;
            }
        }
        else if (TileType == TileType.MachineFeederRight || TileType == TileType.MachineFeederDown || TileType == TileType.MachineFeederLeft || TileType == TileType.MachineFeederUp)
        {
            _feedingMachine.Visible = true;

            switch (TileType)
            {
                case TileType.MachineFeederUp: _feedingMachine.RotationDegrees = 270; break;
                case TileType.MachineFeederRight: _feedingMachine.RotationDegrees = 0; break;
                case TileType.MachineFeederDown: _feedingMachine.RotationDegrees = 90; break;
                case TileType.MachineFeederLeft: _feedingMachine.RotationDegrees = 180; break;
            }
        }
        else if (TileType == TileType.Brick)
        {
            _brick.Visible = true;
        }
    }

    private void ResetVisibility()
    {
        _treadmill.Visible = false;
        _jonction.Visible = false;
        _input.Visible = false;
        _output.Visible = false;
        _washingMaching.Visible = false;
        _feedingMachine.Visible = false;
    }
}
