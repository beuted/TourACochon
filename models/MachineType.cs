// Shoudl follow the same order as the order in tile-buttons.png
using System;
using System.CodeDom;

public enum MachineType
{
  Treadmill = 0,
  Jonction = 1,
  Output = 2,
  Input = 3,
  MachineWasher = 4,
}

public static class MachineTypeExtension
{
  public static TileType GetTileType(this MachineType machineType, Direction direction)
  {
    switch (machineType)
    {
      case MachineType.Jonction:
        return TileType.Jonction; // pas de direction pour une jonction
      case MachineType.Treadmill:
        switch (direction)
        {
          case Direction.Right:
            return TileType.TreadmillRight;
          case Direction.Down:
            return TileType.TreadmillDown;
          case Direction.Left:
            return TileType.TreadmillLeft;
          case Direction.Up:
            return TileType.TreadmillUp;
          default: throw new Exception("GetTileType unknown");
        }
      case MachineType.Input:
        switch (direction)
        {
          case Direction.Right:
            return TileType.InputRight;
          case Direction.Down:
            return TileType.InputDown;
          case Direction.Left:
            return TileType.InputLeft;
          case Direction.Up:
            return TileType.InputUp;
          default: throw new Exception("GetTileType unknown");
        }
      case MachineType.Output:
        switch (direction)
        {
          case Direction.Right:
            return TileType.OutputRight;
          case Direction.Down:
            return TileType.OutputDown;
          case Direction.Left:
            return TileType.OutputLeft;
          case Direction.Up:
            return TileType.OutputUp;
          default: throw new Exception("GetTileType unknown");
        }
      case MachineType.MachineWasher:
        switch (direction)
        {
          case Direction.Right:
            return TileType.MachineWasherRight;
          case Direction.Down:
            return TileType.MachineWasherDown;
          case Direction.Left:
            return TileType.MachineWasherLeft;
          case Direction.Up:
            return TileType.MachineWasherUp;
          default: throw new Exception("GetTileType unknown");
        }
      default: throw new Exception("GetTileType unknown");
    }
  }
}
