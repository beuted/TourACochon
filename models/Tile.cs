using System;

public class Tile
{
    public TileType Type;
    public Func<Item, Item> Produces;
    public Func<Item, bool> Consumes;

    public Tile(TileType type, Func<Item, Item> generate = null, Func<Item, bool> consume = null)
    {
        Type = type;
        Produces = generate ?? _generateNothing;
        Consumes = consume ?? _consumeNothing;
    }

    private static readonly Func<Item, Item> _generateNothing = (_) => null;
    private static readonly Func<Item, bool> _consumeNothing = (_) => true;
}
