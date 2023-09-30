using System;
using System.Collections.Generic;

public class Tile
{
    public TileType Type;
    public Dictionary<PigPerks, int> Inputs = new Dictionary<PigPerks, int>();
    public Recipe Recipe;

    public Tile(TileType type)
    {
        Type = type;
        Recipe = type.GetRecipe();
    }
}
