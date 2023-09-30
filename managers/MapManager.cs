using Godot;
using System;

public class MapManager : Node
{
    public static float TileSize = 16f;
    public static float Speed = 100f;


    private TileMap _tileMap;
    private Node2D _itemsContainer;


    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        Tick(delta);
    }

    public void Init(TileMap tileMap, Node2D itemsContainer)
    {
        _tileMap = tileMap;
        _itemsContainer = itemsContainer;
    }

    public void Tick(float delta)
    {
        foreach (var obj in _itemsContainer.GetChildren())
        {
            var item = obj as Item;

            var cell = _tileMap.GetCell((int)(item.Position.x / TileSize), (int)(item.Position.y / TileSize));

            GD.Print(cell);

            if (cell == -1)
                continue;

            item.Position += GetItemMotion(cell, delta);
        }
    }

    private Vector2 GetItemMotion(int cell, float delta)
    {
        TileType tileType = (TileType)cell;
        switch (tileType)
        {
            case TileType.TreadmillUp: return new Vector2(0, -delta * Speed);
            case TileType.TreadmillRight: return new Vector2(delta * Speed, 0);
            case TileType.TreadmillDown: return new Vector2(0, delta * Speed);
            case TileType.TreadmillLeft: return new Vector2(-delta * Speed, 0);
            case TileType.Jonction: return new Vector2(0, 0); //TODO
            default: throw new Exception("GetItemMotion case not handled !!!!!!!");
        }
    }

}
