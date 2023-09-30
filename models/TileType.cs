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
}

public static class TileTypeExtension
{
  public static ItemType? ItemGenerated(this TileType tileType)
  {
    switch (tileType)
    {
      case TileType.InputUp:
      case TileType.InputRight:
      case TileType.InputDown:
      case TileType.InputLeft:
        return ItemType.CochonSale;
      default: return null;
    }
  }
}