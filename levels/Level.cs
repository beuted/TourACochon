using Godot;
using System;

public class Level : TileMap
{
    [Export]
    public int NbTreadmills = 0;

    [Export]
    public int NbMachineWashs = 0;

    [Export]
    public int NbMachineFeeds = 0;

    [Export]
    public int NbJonctions = 0;

    [Export]
    public int NbItemToWin = 1;

    [Export]
    public PigPerks TypeOfItemToWin = PigPerks.None;

    [Export]
    public PigPerks TypeOfItemInput1 = PigPerks.None;

    [Export]
    public PigPerks TypeOfItemInput2 = PigPerks.None;

    [Export]
    public PigPerks TypeOfItemInput3 = PigPerks.None;

    public override void _Ready()
    {

    }
}
