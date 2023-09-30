using Godot;
using System;
using System.Collections.Generic;

public class MapManager : Node
{
    public static float TileSize = 16f;
    public static float Speed = 100f;
    public static ulong TimeBetweenTurnMs = 1000;
    public static ulong _lastUpdateTimeMs;

    private TileMap _tileMap;
    private Dictionary<Vector2i, Tile> _tileDictionary = new Dictionary<Vector2i, Tile>(); // This structure has been added to hold more info than the simple tileMap. It's initiated with it
    private Node2D _itemsContainer;
    private bool _initialized = false;

    private CameraManager _cameraManager;

    public PackedScene _itemScene = ResourceLoader.Load<PackedScene>("res://entities/Item.tscn");

    public override void _Ready()
    {
        // Autoloads
        _cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton

        _lastUpdateTimeMs = OS.GetSystemTimeMsecs();
    }

    public override void _Process(float delta)
    {
        if (!_initialized)
        {
            return;
        }

        var currentTime = OS.GetSystemTimeMsecs();
        if (currentTime > _lastUpdateTimeMs + TimeBetweenTurnMs)
        {
            Tick();

            _cameraManager.AddTrauma(0.3f);
            _lastUpdateTimeMs = _lastUpdateTimeMs + TimeBetweenTurnMs;
        }
    }

    public void Init(TileMap tileMap, Node2D itemsContainer)
    {
        _tileMap = tileMap;
        _itemsContainer = itemsContainer;
        _initialized = true;

        // Init _cellmap based on _tileMap
        foreach (var obj in _tileMap.GetUsedCells())
        {
            var cell = obj as Vector2?;
            if (!cell.HasValue)
                continue;

            var tileType = (TileType)_tileMap.GetCell((int)cell.Value.x, (int)cell.Value.y);
            _tileDictionary[new Vector2i((int)cell.Value.x, (int)cell.Value.y)] = new Tile()
            {
                Type = tileType,
                Generate = tileType.ItemGenerated()
            };
        }
    }

    public void Tick()
    {
        // Setup new Destinationf or each items
        foreach (var obj in _itemsContainer.GetChildren())
        {
            var item = obj as Item;

            var cellPosi = new Vector2i((int)(item.Position.x / TileSize), (int)(item.Position.y / TileSize));
            var cell = _tileMap.GetCell(cellPosi.X, cellPosi.Y);

            if (cell == -1)
                continue;

            item.Direction = GetItemNewDirection(cell, item.Direction);
            item.Destination = GetItemNewDestination(cellPosi, item.Direction);
        }

        // Create items out of inputs tiles
        foreach (var posTile in _tileDictionary.Keys)
        {
            var tile = _tileDictionary[posTile];
            if (!tile.Generate.HasValue)
                continue;

            var newItem = _itemScene.Instance<Item>();
            newItem.Type = tile.Generate.Value;
            newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);

            _itemsContainer.AddChild(newItem);
        }
    }

    private Vector2 GetItemNewDirection(int cell, Vector2 previousDirection)
    {
        TileType tileType = (TileType)cell;

        switch (tileType)
        {
            case TileType.TreadmillUp:
            case TileType.InputUp:
                return Vector2.Up;
            case TileType.TreadmillRight:
            case TileType.InputRight:
                return Vector2.Right;
            case TileType.TreadmillDown:
            case TileType.InputDown:
                return Vector2.Down;
            case TileType.TreadmillLeft:
            case TileType.InputLeft:
                return Vector2.Left;
            case TileType.Jonction: return previousDirection;
            default: throw new Exception("GetItemNewDestination case not handled !!!!!!!");
        }
    }

    private Vector2 GetItemNewDestination(Vector2i cellPosi, Vector2 direction)
    {
		var currentPositionCenteredOnCell = new Vector2(cellPosi.X * TileSize + TileSize / 2, cellPosi.Y * TileSize + TileSize / 2);

        return currentPositionCenteredOnCell + (direction * TileSize);
    }
}
