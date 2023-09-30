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

			item.Direction = GetItemNewDirection(cell, item.Direction);
			item.Destination = GetItemNewDestination(cellPosi, item.Direction);
		}
	}

	private Vector2 GetItemNewDirection(int cell, Vector2 previousDirection)
	{
		TileType tileType = (TileType)cell;

		switch (tileType)
		{
			case TileType.TreadmillUp: return Vector2.Up;
			case TileType.TreadmillRight: return Vector2.Right;
			case TileType.TreadmillDown: return Vector2.Down;
			case TileType.TreadmillLeft: return Vector2.Left;
			case TileType.Jonction: return previousDirection;
			default: throw new Exception("GetItemNewDestination case not handled !!!!!!!");
		}
	}

	private Vector2 GetItemNewDestination(Vector2i cellPosi, Vector2 direction)
	{
		var currentPositionCenteredOnCell = new Vector2(cellPosi.X * 16f + 8f, cellPosi.Y * 16f + 8f);

		return currentPositionCenteredOnCell + (direction * TileSize);
	}
}
