using Godot;
using System;
using System.Collections.Generic;

public class MapManager : Node
{
    public static float TileSize = 32f;
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

        // Init _tileDictionary based on _tileMap
        foreach (var obj in _tileMap.GetUsedCells())
        {
			var cellVect2 = obj as Vector2?;
			if (!cellVect2.HasValue)
				continue;
			var cell = (Vector2i)cellVect2.Value;

			var tileType = (TileType)_tileMap.GetCell(cell.X, cell.Y);

			_tileDictionary[cell] = CreateTile(tileType);
		}
	}

	public Tile CreateTile(TileType tileType)
	{
		Func<Item, Item> producer = null;
		if (tileType.ProducesWithoutInput())
		{
			producer = (Item _) => _itemScene.Instance<Item>();
		}

		Func<Item, bool> consumer = null;
		if (tileType.Consumes())
		{
			// TODO: Change this to support the different perks as input and reject them or not based on the machine type
			consumer = (Item _) => true;
		}

		return new Tile(tileType, producer, consumer);
	}

	public void Tick()
	{
		// Setup new Destinationf or each items
		foreach (var obj in _itemsContainer.GetChildren())
		{
			var item = obj as Item;

			Vector2i cellPosi = item.Position / TileSize;
			var cell = _tileMap.GetCell(cellPosi.X, cellPosi.Y);

			if (cell == -1)
                continue;

			var tileType = (TileType)cell;

			if (tileType.Consumes())
			{
				if (_tileDictionary[cellPosi].Consumes(item))
				{
					GD.Print("Consumed ; TODO: Increment counter, add overlay");
				}
				else
				{
					GD.Print("Rejected ; TODO: Add overlay");
				}

				item.QueueFree();
				continue;
			}

			item.Direction = GetItemNewDirection(tileType, item.Direction);
			item.Destination = GetItemNewDestination(cellPosi, item.Direction);
		}

		// Create items out of inputs tiles
		foreach (var posTile in _tileDictionary.Keys)
		{
			var tile = _tileDictionary[posTile];
			if (!tile.Type.ProducesWithoutInput())
				continue;

			var newItem = tile.Produces(null);
			newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);

			_itemsContainer.AddChild(newItem);
		}
	}

	private Vector2 GetItemNewDirection(TileType tileType, Vector2 previousDirection)
	{
		switch (tileType)
		{
			case TileType.TreadmillUp:
			case TileType.InputUp:
			case TileType.OutputDown:
				return Vector2.Up;
			case TileType.TreadmillRight:
			case TileType.InputRight:
			case TileType.OutputLeft:
				return Vector2.Right;
			case TileType.TreadmillDown:
			case TileType.InputDown:
			case TileType.OutputUp:
				return Vector2.Down;
			case TileType.TreadmillLeft:
			case TileType.InputLeft:
			case TileType.OutputRight:
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
