// Doit respecter l'ordre de définition des tiles du tileset
using System;
using System.Collections.Generic;

public enum TileType
{
  TreadmillUp = 0,
  TreadmillRight = 1,
  TreadmillDown = 2,
  TreadmillLeft = 3,
  Jonction = 4,
  InputUp = 5,
  InputRight = 6,
  InputDown = 7,
  InputLeft = 8,
  // Conveyor == 9
  OutputRight = 10,
  OutputDown = 11,
  OutputLeft = 12,
  OutputUp = 13,
  MachineWasherRight = 14,
  MachineWasherUp = 15,
  MachineWasherDown = 16,
  MachineWasherLeft = 17,
  Brick = 18,
  MachineFeederRight = 19,
  MachineFeederUp = 20,
  MachineFeederDown = 21,
  MachineFeederLeft = 22,
}

public static class TileTypeExtension
{
  public static bool ProducesWithoutInput(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.InputUp:
      case TileType.InputRight:
      case TileType.InputDown:
      case TileType.InputLeft:
        return true;
      default: return false;
    }
  }

  public static Recipe GetRecipe(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.OutputUp:
      case TileType.OutputRight:
      case TileType.OutputDown:
      case TileType.OutputLeft:
        return new Recipe
        {
          Input = new Dictionary<PigPerks, int>() { }, // Replaced dynamically at level init
          Output = new List<PigPerks>(),
        };
      case TileType.MachineWasherLeft:
      case TileType.MachineWasherRight:
      case TileType.MachineWasherUp:
      case TileType.MachineWasherDown:
        return new Recipe
        {
          Input = new Dictionary<PigPerks, int>()
          {
            [PigPerks.None] = 1,
          },
          Output = new List<PigPerks>() {
            PigPerks.Cleaned,
          },
        };
      case TileType.MachineFeederLeft:
      case TileType.MachineFeederRight:
      case TileType.MachineFeederUp:
      case TileType.MachineFeederDown:
        return new Recipe
        {
          Input = new Dictionary<PigPerks, int>()
          {
            [PigPerks.Cleaned] = 1,
            [PigPerks.PigFood] = 1,
          },
          Output = new List<PigPerks>() {
            PigPerks.Fat,
          },
        };
      default: return null;
    }
  }

  public static MachineType GetMachineType(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.Jonction:
        return MachineType.Jonction;
      case TileType.TreadmillRight:
      case TileType.TreadmillDown:
      case TileType.TreadmillLeft:
      case TileType.TreadmillUp:
        return MachineType.Treadmill;
      case TileType.InputRight:
      case TileType.InputDown:
      case TileType.InputLeft:
      case TileType.InputUp:
        return MachineType.Input;
      case TileType.OutputRight:
      case TileType.OutputDown:
      case TileType.OutputLeft:
      case TileType.OutputUp:
        return MachineType.Output;
      case TileType.MachineWasherRight:
      case TileType.MachineWasherDown:
      case TileType.MachineWasherLeft:
      case TileType.MachineWasherUp:
        return MachineType.MachineWasher;
      case TileType.MachineFeederRight:
      case TileType.MachineFeederDown:
      case TileType.MachineFeederLeft:
      case TileType.MachineFeederUp:
        return MachineType.MachineFeeder;
      case TileType.Brick:
        return MachineType.Brick;
      default: throw new Exception("GetMachineType unknown");
    }
  }

  public static Direction GetDirection(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.TreadmillRight:
      case TileType.InputRight:
      case TileType.OutputRight:
      case TileType.MachineWasherRight:
      case TileType.MachineFeederRight:
        return Direction.Right;
      case TileType.TreadmillDown:
      case TileType.InputDown:
      case TileType.OutputDown:
      case TileType.MachineWasherDown:
      case TileType.MachineFeederDown:
        return Direction.Down;
      case TileType.TreadmillLeft:
      case TileType.InputLeft:
      case TileType.OutputLeft:
      case TileType.MachineWasherLeft:
      case TileType.MachineFeederLeft:
        return Direction.Left;
      case TileType.TreadmillUp:
      case TileType.InputUp:
      case TileType.OutputUp:
      case TileType.MachineWasherUp:
      case TileType.MachineFeederUp:
        return Direction.Up;
      case TileType.Jonction: return Direction.Same;
      default: throw new Exception("GetDirection unknown");
    }
  }
}
