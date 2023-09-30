// Doit respecter l'ordre de définition des tiles du tileset
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
  OutputUp = 9,
  OutputRight = 10,
  OutputDown = 11,
  OutputLeft = 12,
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

  public static bool Consumes(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.OutputUp:
      case TileType.OutputRight:
      case TileType.OutputDown:
      case TileType.OutputLeft:
        return true;
      default: return false;
    }
  }
}