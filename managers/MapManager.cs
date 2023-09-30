using Godot;
using System;

public class MapManager : Node
{
    public static float TileSize = 16f;
    public static float Speed = 100f;
    public static ulong TimeBetweenTurnMs = 1000;
    public static ulong _lastUpdateTimeMs;

    private TileMap _tileMap;
    private Node2D _itemsContainer;
    private bool _initialized = false;

    private CameraManager _cameraManager;

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
    }

    public void Tick()
    {
        foreach (var obj in _itemsContainer.GetChildren())
        {
            var item = obj as Item;

            var cellPosi = new Vector2i((int)(item.Position.x / TileSize), (int)(item.Position.y / TileSize));
            var cell = _tileMap.GetCell(cellPosi.X, cellPosi.Y);

            if (cell == -1)
                continue;

            item.Destination = GetItemNewDestination(cellPosi, cell);
        }
    }

    private Vector2 GetItemNewDestination(Vector2i cellPosi, int cell)
    {
        TileType tileType = (TileType)cell;

        var currentPositionCenteredOnCell = new Vector2(cellPosi.X * 16f + 8f, cellPosi.Y * 16f + 8f);

        switch (tileType)
        {
            case TileType.TreadmillUp: return currentPositionCenteredOnCell + new Vector2(0, -TileSize);
            case TileType.TreadmillRight: return currentPositionCenteredOnCell + new Vector2(TileSize, 0);
            case TileType.TreadmillDown: return currentPositionCenteredOnCell + new Vector2(0, TileSize);
            case TileType.TreadmillLeft: return currentPositionCenteredOnCell + new Vector2(-TileSize, 0);
            case TileType.Jonction: return currentPositionCenteredOnCell; //TODO
            default: throw new Exception("GetItemNewDestination case not handled !!!!!!!");
        }
    }

}
