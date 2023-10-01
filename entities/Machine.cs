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
    private Node2D _treadmillUp;
    private Node2D _treadmillDown;
    private Node2D _jonction;
    private Node2D _input;
    private Node2D _output;
    private Sprite _treadmillSprite;
    private Sprite _treadmillUpSprite;
    private Sprite _treadmillDownSprite;
    private Sprite _washingMachingSprite;
    private Sprite _feedingMachingSprite;
    private Node2D _washingMaching;
    private Node2D _feedingMachine;
    private Node2D _brick;
    private TileType _tileType;

    public override void _Ready()
    {
        _treadmill = GetNode<Node2D>("Treadmill");
        _treadmillUp = GetNode<Node2D>("TreadmillUp");
        _treadmillDown = GetNode<Node2D>("TreadmillDown");
        _washingMaching = GetNode<Node2D>("MachineWasher");
        _feedingMachine = GetNode<Node2D>("MachineFeeder");
        _jonction = GetNode<Node2D>("Jonction");
        _input = GetNode<Node2D>("Input");
        _output = GetNode<Node2D>("Output");
        _brick = GetNode<Node2D>("Brick");

        _treadmillSprite = GetNode<Sprite>("Treadmill/Sprite");
        _treadmillUpSprite = GetNode<Sprite>("TreadmillUp/Sprite");
        _treadmillDownSprite = GetNode<Sprite>("TreadmillDown/Sprite");
        _washingMachingSprite = GetNode<Sprite>("MachineWasher/Sprite");
        _feedingMachingSprite = GetNode<Sprite>("MachineFeeder/Sprite");

        OnTileTypeChanged();
    }

    public override void _Process(float delta)
    {
        if (TileType == TileType.TreadmillRight || TileType == TileType.TreadmillLeft)
        {
            var time = OS.GetSystemTimeMsecs();
            _treadmillSprite.Frame = (int)((time % 400ul) / 50ul); // This is done instead of using an animtation player in order to keep all treadmills in syn visually
        }
        if (TileType == TileType.TreadmillUp)
        {
            var time = OS.GetSystemTimeMsecs();
            _treadmillUpSprite.Frame = (int)((time % 400ul) / 50ul); // This is done instead of using an animtation player in order to keep all treadmills in syn visually
        }
        if (TileType == TileType.TreadmillDown)
        {
            var time = OS.GetSystemTimeMsecs();
            _treadmillDownSprite.Frame = (int)((time % 400ul) / 50ul); // This is done instead of using an animtation player in order to keep all treadmills in syn visually
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

        if (TileType == TileType.TreadmillRight || TileType == TileType.TreadmillLeft)
        {
            _treadmill.Visible = true;
            switch (TileType)
            {
                case TileType.TreadmillRight: _treadmillSprite.FlipH = false; break;
                case TileType.TreadmillLeft: _treadmillSprite.FlipH = true; break;
            }
        }
        else if (TileType == TileType.TreadmillUp)
        {
            _treadmillUp.Visible = true;
        }
        else if (TileType == TileType.TreadmillDown)
        {
            _treadmillDown.Visible = true;
        }
        else if (TileType == TileType.Jonction)
        {
            _jonction.Visible = true;
        }
        else if (TileType == TileType.InputUp || TileType == TileType.InputRight || TileType == TileType.InputDown || TileType == TileType.InputLeft)
        {
            _input.Visible = true;
        }
        else if (TileType == TileType.OutputRight || TileType == TileType.OutputDown || TileType == TileType.OutputLeft || TileType == TileType.OutputUp)
        {
            _output.Visible = true;
        }
        else if (TileType == TileType.MachineWasherRight || TileType == TileType.MachineWasherDown || TileType == TileType.MachineWasherLeft || TileType == TileType.MachineWasherUp)
        {
            _washingMaching.Visible = true;

            switch (TileType)
            {
                case TileType.MachineWasherUp: _washingMachingSprite.Frame = 3; break;
                case TileType.MachineWasherRight: _washingMachingSprite.Frame = 1; break;
                case TileType.MachineWasherDown: _washingMachingSprite.Frame = 0; break;
                case TileType.MachineWasherLeft: _washingMachingSprite.Frame = 2; break;
            }
        }
        else if (TileType == TileType.MachineFeederRight || TileType == TileType.MachineFeederDown || TileType == TileType.MachineFeederLeft || TileType == TileType.MachineFeederUp)
        {
            _feedingMachine.Visible = true;

            switch (TileType)
            {
                case TileType.MachineFeederUp: _feedingMachingSprite.Frame = 3; break;
                case TileType.MachineFeederRight: _feedingMachingSprite.Frame = 1; break;
                case TileType.MachineFeederDown: _feedingMachingSprite.Frame = 0; break;
                case TileType.MachineFeederLeft: _feedingMachingSprite.Frame = 2; break;
            }
        }
        else if (TileType == TileType.Brick)
        {
            _brick.Visible = true;
        }

        GD.Print(_treadmillSprite.FlipH, ", ", _treadmill.Visible);

    }

    private void ResetVisibility()
    {
        _treadmill.Visible = false;
        _treadmillUp.Visible = false;
        _treadmillDown.Visible = false;
        _washingMaching.Visible = false;
        _feedingMachine.Visible = false;
        _jonction.Visible = false;
        _input.Visible = false;
        _output.Visible = false;
        _washingMaching.Visible = false;
        _feedingMachine.Visible = false;
        _brick.Visible = false;
    }
}
