using Godot;
using System;
using System.Collections.Generic;

public class MapManager : Node
{
	public static float TileSize = 32f;
	public static float Speed = 100f;
	public static ulong TimeBetweenTurnMs = 1000;
	public static ulong _lastUpdateTimeMs;

	private Node2D _itemsContainer;
	private Node2D _tilesContainer;

	// This structure has been added to hold more info than the simple tileMap. It's initiated with it
	private Dictionary<Vector2i, Tile> _tileDictionary = new Dictionary<Vector2i, Tile>();
	// This structure has been added to the actual Node2d of the tiles we show on the map, this is to deal with animated tiles and special effetcs that are not well supported on tilemap
	private Dictionary<Vector2i, Machine> _machineDictionary = new Dictionary<Vector2i, Machine>();
	private bool _initialized = false;

	private CameraManager _cameraManager;

	public PackedScene _itemScene = ResourceLoader.Load<PackedScene>("res://entities/Item.tscn");
	public PackedScene _machineScene = ResourceLoader.Load<PackedScene>("res://entities/Machine.tscn");

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

	public void Init(TileMap tileMap, Node2D itemsContainer, Node2D tilesContainer)
	{
		tileMap.Visible = false; // We hide the tilemap because evryhting is shown in tilesContainer now

		_itemsContainer = itemsContainer;
		_tilesContainer = tilesContainer;

		_initialized = true;

		// Init _tileDictionary based on _tileMap
		foreach (var obj in tileMap.GetUsedCells())
		{
			var cellVect2 = obj as Vector2?;
			if (!cellVect2.HasValue)
				continue;
			var cell = (Vector2i)cellVect2.Value;

			var tileType = (TileType)tileMap.GetCell(cell.X, cell.Y);

			_tileDictionary[cell] = new Tile(tileType);


			// Instanciate a machine to add on the map
			var newMachine = _machineScene.Instance<Machine>();
			newMachine.Position = new Vector2(cell.X * TileSize, cell.Y * TileSize);
			newMachine.TileType = tileType;

			_tilesContainer.AddChild(newMachine);
			_machineDictionary[cell] = newMachine;
		}
	}

	public void Tick()
	{
		// Setup new Destination for each items
		foreach (var obj in _itemsContainer.GetChildren())
		{
			var item = obj as Item;

			Vector2i cellPosi = item.Position / TileSize;

			if (!_tileDictionary.TryGetValue(cellPosi, out var cell))
				continue;

			if (_tileDictionary.ContainsKey(cellPosi) && cell.Recipe != null)
			{
				if (!cell.Recipe.Input.ContainsKey(item.Perks))
				{
					GD.Print("Item rejected, TODO: Print a message to the player");
				}
				else
				{
					if (!cell.Inputs.ContainsKey(item.Perks))
						cell.Inputs[item.Perks] = 0;

					cell.Inputs[item.Perks] += 1;
				}

				item.QueueFree();
				continue;
			}

			var tileType = cell.Type;
			item.Direction = GetItemNewDirection(tileType, item.Direction);
			item.Destination = GetItemNewDestination(cellPosi, item.Direction);
		}

		// Create items out of inputs tiles
		foreach (var posTile in _tileDictionary.Keys)
		{
			var tile = _tileDictionary[posTile];
			if (tile.Type.ProducesWithoutInput())
			{
				var newItem = _itemScene.Instance<Item>();
				newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);
				_itemsContainer.AddChild(newItem);
				continue;
			}

			if (tile.Recipe == null)
			{
				continue;
			}

			var numberOfRecipes = 0;
			foreach (var recipePerk in tile.Recipe.Input)
			{
				if (!tile.Inputs.TryGetValue(recipePerk.Key, out var inputCount))
				{
					// Mandatory recipe input missing, nothing to do
					numberOfRecipes = 0;
					break;
				}

				numberOfRecipes = Math.Min(numberOfRecipes, inputCount / recipePerk.Value);
			}

			// For each count of recipe, create an item and remove the inputs
			for (var i = 0; i < numberOfRecipes; ++i)
			{
				foreach (var recipePerk in tile.Recipe.Input)
				{
					tile.Inputs[recipePerk.Key] -= recipePerk.Value;
					if (tile.Inputs[recipePerk.Key] == 0)
					{
						tile.Inputs.Remove(recipePerk.Key);
					}
				}

				var newItem = _itemScene.Instance<Item>();
				newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);
				_itemsContainer.AddChild(newItem);
			}
		}
	}

	public bool PlaceMachine(Vector2i pos, MachineType type, Direction direction)
	{
		if (pos.X < 0 || pos.X > 10 || pos.Y < 0 || pos.Y > 10)
		{
			GD.Print("PlaceMachine failed: out of boundaries");
			return false;
		}

		if (_tileDictionary.TryGetValue(pos, out var tile))
		{
			GD.Print("PlaceMachine failed: already a tile there");
			return false;
		}


		//TODO set the tile (_tileDictionary[pos] =  ...)
		GD.Print("PlaceMachine ", pos, ", ", type);

		// Instanciate a machine to add on the map
		var newMachine = _machineScene.Instance<Machine>();
		newMachine.Position = new Vector2(pos.X * TileSize, pos.Y * TileSize);
		newMachine.TileType = type.GetTileType(direction);

		_tilesContainer.AddChild(newMachine);
		_machineDictionary[pos] = newMachine;

		return true; // sucess
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
