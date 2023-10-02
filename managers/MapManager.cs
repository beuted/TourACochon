using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MapManager : Node
{
	public static float TileSize = 32f;
	public static float Speed = 100f;
	public static ulong TimeBetweenTurnMs = 1000;
	public static ulong _lastUpdateTimeMs;
	public static int NbLevel = 5;

	private List<Level> _prefabLevels = new List<Level>();

	private Node2D _itemsContainer;
	private Node2D _tilesContainer;
	private Machine _potentialMachine = null;

	// This structure has been added to hold more info than the simple tileMap. It's initiated with it
	private Dictionary<Vector2i, Tile> _tileDictionary = new Dictionary<Vector2i, Tile>();
	// This structure has been added to the actual Node2d of the tiles we show on the map, this is to deal with animated tiles and special effetcs that are not well supported on tilemap
	private Dictionary<Vector2i, Machine> _machineDictionary = new Dictionary<Vector2i, Machine>();
	private bool _initialized = false;

	private CameraManager _cameraManager;
	private TileBuilderManager _tileBuilderManager;
	private GameProgressManager _gameProgressManager;
	private SoundManager _soundManager;

	public PackedScene _itemScene = ResourceLoader.Load<PackedScene>("res://entities/Item.tscn");
	public PackedScene _machineScene = ResourceLoader.Load<PackedScene>("res://entities/Machine.tscn");

	public override void _Ready()
	{
		// Autoloads
		_cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton
		_tileBuilderManager = (TileBuilderManager)GetNode($"/root/{nameof(TileBuilderManager)}"); // Singleton
		_gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
		_soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton

		_lastUpdateTimeMs = OS.GetSystemTimeMsecs();

		for (var i = 0; i <= NbLevel - 1; i++)
		{
			_prefabLevels.Add(ResourceLoader.Load<PackedScene>("res://levels/Level" + i + ".tscn").Instance() as Level);
		}
	}

	public override void _Process(float delta)
	{
		if (!_initialized || _gameProgressManager.GameWon)
		{
			return;
		}

		var currentTime = OS.GetSystemTimeMsecs();
		if (currentTime > _lastUpdateTimeMs + TimeBetweenTurnMs)
		{
			Tick();

			//_cameraManager.AddTrauma(0.3f);
			_lastUpdateTimeMs = _lastUpdateTimeMs + TimeBetweenTurnMs;
		}
	}

	public void Init(TileMap tileMap, Node2D itemsContainer, Node2D tilesContainer)
	{
		tileMap.Visible = false; // We hide the tilemap because evryhting is shown in tilesContainer now

		_itemsContainer = itemsContainer;
		_tilesContainer = tilesContainer;

		if (_potentialMachine == null)
		{
			_potentialMachine = _machineScene.Instance<Machine>();
			_potentialMachine.Modulate = new Color("49a1ff1e");
			_potentialMachine.ZIndex = 2000;
			_tilesContainer.AddChild(_potentialMachine);
			_potentialMachine.Visible = false;
		}

		InitLevel(0);

		// Set init to true
		_initialized = true;
	}

	public void ClearLevel()
	{
		// Destroy all machines
		foreach (var pos in _tileDictionary.Keys.ToList())
		{
			DestroyMachine(pos, true);
		}

		// Destory all items
		ResetItems();
	}

	public void ResetStartPipeOutputs()
	{
		if (_tileDictionary.TryGetValue(new Vector2i(2, 2), out _))
		{
			_tileDictionary[new Vector2i(2, 2)].Outputs = new List<PigPerks>() {
				PigPerks.None, PigPerks.None, PigPerks.None, PigPerks.None, PigPerks.None
			};
			_machineDictionary[new Vector2i(2, 2)].ItemProduced = PigPerks.None;
		}

		if (_tileDictionary.TryGetValue(new Vector2i(2, 4), out _))
		{
			_tileDictionary[new Vector2i(2, 4)].Outputs = new List<PigPerks>() {
				PigPerks.PigFood, PigPerks.PigFood, PigPerks.PigFood, PigPerks.PigFood, PigPerks.PigFood
			};
			_machineDictionary[new Vector2i(2, 4)].ItemProduced = PigPerks.PigFood;
		}

	}

	public void ResetItems()
	{
		// Destory all items
		foreach (var obj in _itemsContainer.GetChildren())
		{
			var item = obj as Item;
			item.QueueFree();
		}

		// Reset the inputs so that they emit items when start is pressed
		ResetStartPipeOutputs();
	}

	public void InitLevel(int levelId)
	{
		// Clear the level first
		ClearLevel();

		var levelPrefab = _prefabLevels[levelId];

		// Init the tiles thta will be available
		_tileBuilderManager.Init(new Dictionary<MachineType, int>(){
			{ MachineType.Input, 0 },
			{ MachineType.Output, 0 },
			{ MachineType.Treadmill, levelPrefab.NbTreadmills },
			{ MachineType.Jonction, levelPrefab.NbJonctions },
			{ MachineType.MachineWasher, levelPrefab.NbMachineWashs },
			{ MachineType.MachineFeeder, levelPrefab.NbMachineFeeds },
		});

		// Init victory conditions
		_gameProgressManager.Init(levelPrefab.NbItemToWin, levelPrefab.TypeOfItemToWin);

		// Init _tileDictionary based on _tileMap
		foreach (var obj in levelPrefab.GetUsedCells())
		{
			var cellVect2 = obj as Vector2?;
			if (!cellVect2.HasValue)
				continue;
			var cell = (Vector2i)cellVect2.Value;

			// We only copy what is within the map
			if (cell.X < 0 || cell.Y < 1 || cell.X > 10 || cell.Y > 5)
				continue;

			var offsetedCell = new Vector2i(cell.X + 2, cell.Y + 0); //TODO: we offset the level to cetner it, that's hacky buy hey

			var tileType = (TileType)levelPrefab.GetCell(cell.X, cell.Y);

			_tileDictionary[offsetedCell] = new Tile(tileType);
			var machineType = tileType.GetMachineType();
			if (machineType == MachineType.Output)
			{
				// Semi-Hack: we set the recipe of the output depending on the level here
				_tileDictionary[offsetedCell].Recipe.Input = new Dictionary<PigPerks, int>()
				{
					[levelPrefab.TypeOfItemToWin] = 3,
				};
			}

			// Instanciate a machine to add on the map
			var newMachine = _machineScene.Instance<Machine>();
			newMachine.Position = new Vector2(offsetedCell.X * TileSize, offsetedCell.Y * TileSize);
			newMachine.TileType = tileType;
			newMachine.IsCreatedByUser = false;

			if (machineType != MachineType.Output && machineType != MachineType.Input && machineType != MachineType.Brick)
			{
				newMachine.Modulate = new Color("d48e8e");
			}

			_tilesContainer.AddChild(newMachine);
			_machineDictionary[offsetedCell] = newMachine;
			if (machineType == MachineType.Output)
			{
				_machineDictionary[offsetedCell].ItemProduced = levelPrefab.TypeOfItemToWin;
			}
		}

		// Reset the inputs so that they emit items when start is pressed
		ResetStartPipeOutputs();

		// Set init to true
		_initialized = true;
	}

	public void Tick()
	{
		var hasPlayedPigSpawnSoundThisTick = false;
		var hasPlayedMachineSoundFoodThisTick = false;
		var hasPlayedMachineSoundWaterThisTick = false;

		// No need to outputs any item if the level has not started yet
		if (!_gameProgressManager.InputStarted)
			return;

		// Setup new Destination for each items
		foreach (var obj in _itemsContainer.GetChildren())
		{
			var item = obj as Item;

			Vector2i cellPosi = item.Position / TileSize;

			if (!_tileDictionary.TryGetValue(cellPosi, out var cell))
				continue;

			if (cell.Recipe != null
				// In order not to consume what was just produced, we ignore the items that were outputed by the machine
				&& !cell.Recipe.Output.Contains(item.Perks))
			{
				if (!cell.Recipe.Input.ContainsKey(item.Perks))
				{
					_cameraManager.AddTrauma(0.3f);
					_soundManager.PlayPigScream();
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
			var newDirection = GetItemNewDirection(tileType, item.Direction);
			var newDestination = GetItemNewDestination(cellPosi, newDirection);
			if (!_tileDictionary.TryGetValue(newDestination / TileSize, out var destinationTile) || destinationTile.Type == TileType.Brick)
			{
				// There is no tile at the destination, cannot move there
				continue;
			}

			item.Direction = GetItemNewDirection(tileType, item.Direction);
			item.Destination = GetItemNewDestination(cellPosi, item.Direction);
		}

		// Create items out of inputs tiles
		foreach (var posTile in _tileDictionary.Keys.ToList())
		{
			var tile = _tileDictionary[posTile];
			if (tile.Type.ProducesWithoutInput() && tile.Outputs.Count > 0)
			{
				if (!hasPlayedPigSpawnSoundThisTick)
				{
					_soundManager.PlayPigSpawn();
					hasPlayedPigSpawnSoundThisTick = true;
				}

				var newItem = _itemScene.Instance<Item>();
				newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);
				newItem.Perks = tile.Outputs[0];
				tile.Outputs.RemoveAt(0); // Remove element that we just spawned
				_itemsContainer.AddChild(newItem);
				continue;
			}

			if (tile.Recipe == null)
			{
				continue;
			}

			var numberOfRecipes = int.MaxValue;
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

					if (tile.Type.GetMachineType() == MachineType.Output)
					{
						GD.Print("yo?");
						_gameProgressManager.IncrNbItemGathered();
					}

					if (tile.Inputs[recipePerk.Key] == 0)
					{
						tile.Inputs.Remove(recipePerk.Key);
					}
				}

				foreach (var output in tile.Recipe.Output)
				{
					var newItem = _itemScene.Instance<Item>();
					newItem.Position = new Vector2(posTile.X * TileSize + TileSize / 2, posTile.Y * TileSize + TileSize / 2);
					newItem.Perks = output;
					_itemsContainer.AddChild(newItem);

					if (!hasPlayedMachineSoundWaterThisTick && output == PigPerks.Cleaned)
					{
						GD.Print("hasPlayedMachineSoundWaterThisTick");
						_soundManager.PlayMachineWater();
						hasPlayedMachineSoundWaterThisTick = true;
					}
					else if (!hasPlayedMachineSoundFoodThisTick && output == PigPerks.Fat)
					{
						GD.Print("hasPlayedMachineSoundFoodThisTick");

						_soundManager.PlayMachineFood();
						hasPlayedMachineSoundFoodThisTick = true;
					}
				}
			}
		}
	}

	public bool TryGetTileType(Vector2i pos, out TileType type)
	{
		if (!_tileDictionary.TryGetValue(pos, out var tile))
		{
			type = default;
			return false;
		}

		type = tile.Type;
		return true;
	}

	public void RotateMachine(Vector2i pos)
	{
		if (!_machineDictionary.TryGetValue(pos, out var machine))
		{
			GD.Print("RotateMachine failed: no machine at this position");
			return;
		}

		if (!machine.IsCreatedByUser)
		{
			GD.Print("RotateMachine failed: this machine is not created by the user");
			return;
		}

		var rotatedTileType = GetRotatedTileType(machine.TileType);
		if (rotatedTileType == null)
		{
			// This machine cannot be rotated
			return;
		}

		machine.TileType = rotatedTileType.Value;
		_tileDictionary[pos].Type = rotatedTileType.Value;
	}

	public bool PlaceMachine(Vector2i pos, MachineType machineType)
	{
		if (pos.X < 3 || pos.Y < 1 || pos.X > 11 || pos.Y > 5)
		{
			GD.Print("PlaceMachine failed: out of boundaries");
			return false;
		}

		if (_tileDictionary.TryGetValue(pos, out var tile))
		{
			GD.Print("PlaceMachine failed: already a tile there");
			return false;
		}

		var direction = _potentialMachine.TileType.GetDirection();
		if (direction == Direction.Same)
		{
			direction = Direction.Right;
		}

		var tileType = machineType.GetTileType(direction);
		_tileDictionary[pos] = new Tile(tileType);

		// Instanciate a machine to add on the map
		var newMachine = _machineScene.Instance<Machine>();
		newMachine.Position = new Vector2(pos.X * TileSize, pos.Y * TileSize);
		newMachine.TileType = machineType.GetTileType(direction);
		newMachine.IsCreatedByUser = true;

		_tilesContainer.AddChild(newMachine);
		_machineDictionary[pos] = newMachine;

		_potentialMachine.Visible = false;
		return true; // sucess
	}

	public bool DestroyMachine(Vector2i pos, bool destroySystemMachine = false)
	{
		if (!_tileDictionary.TryGetValue(pos, out var tile))
		{
			GD.Print("Destroy failed: no tile there");
			return false;
		}

		var machine = _machineDictionary[pos];
		if (!machine.IsCreatedByUser && !destroySystemMachine)
		{
			GD.Print("Destroy failed: this machine is not created by the user");
			return false;
		}

		_tileDictionary.Remove(pos);
		_machineDictionary.Remove(pos);
		machine.QueueFree();

		return true;
	}

	private Vector2 GetItemNewDirection(TileType tileType, Vector2 previousDirection)
	{
		var direction = tileType.GetDirection();
		switch (direction)
		{
			case Direction.Up:
				return Vector2.Up;
			case Direction.Right:
				return Vector2.Right;
			case Direction.Down:
				return Vector2.Down;
			case Direction.Left:
				return Vector2.Left;
			case Direction.Same: return previousDirection;
			default: throw new Exception("GetItemNewDestination case not handled !!!!!!!");
		}
	}

	private Vector2 GetItemNewDestination(Vector2i cellPosi, Vector2 direction)
	{
		var currentPositionCenteredOnCell = new Vector2(cellPosi.X * TileSize + TileSize / 2, cellPosi.Y * TileSize + TileSize / 2);

		return currentPositionCenteredOnCell + (direction * TileSize);
	}

	internal void HighlightPotentialMachine(Vector2i tilePos, MachineType machineType)
	{
		if (tilePos.X < 3 || tilePos.Y < 1 || tilePos.X > 11 || tilePos.Y > 5)
		{
			// Out of boundaries
			_potentialMachine.Visible = false;
			return;
		}

		if (TryGetTileType(tilePos, out _))
		{
			// Tile already present
			_potentialMachine.Visible = false;
			return;
		}

		var tileType = machineType.GetTileType(Direction.Right);
		var position = (Vector2)tilePos * TileSize;
		if (_potentialMachine != null && _potentialMachine.TileType.GetMachineType() == machineType && _potentialMachine.Position == position)
		{
			// Already the right machine highlighted, nothing to do
			_potentialMachine.Visible = true;
			return;
		}

		if (tileType != _potentialMachine.TileType)
		{
			_potentialMachine.TileType = tileType;
		}
		_potentialMachine.Position = position;
		_potentialMachine.Visible = true;
	}

	internal void RotateHighlightPotentialMachine()
	{
		if (_potentialMachine == null && _potentialMachine.Visible)
		{
			return;
		}

		var rotatedTileType = GetRotatedTileType(_potentialMachine.TileType);
		if (rotatedTileType == null)
		{
			// This machine cannot be rotated
			return;
		}

		_potentialMachine.TileType = rotatedTileType.Value;
	}

	private static TileType? GetRotatedTileType(TileType tileType)
	{
		switch (tileType)
		{
			case TileType.TreadmillUp: return TileType.TreadmillRight;
			case TileType.TreadmillRight: return TileType.TreadmillDown;
			case TileType.TreadmillDown: return TileType.TreadmillLeft;
			case TileType.TreadmillLeft: return TileType.TreadmillUp;
			case TileType.MachineWasherUp: return TileType.MachineWasherRight;
			case TileType.MachineWasherRight: return TileType.MachineWasherDown;
			case TileType.MachineWasherDown: return TileType.MachineWasherLeft;
			case TileType.MachineWasherLeft: return TileType.MachineWasherUp;
			case TileType.MachineFeederUp: return TileType.MachineFeederRight;
			case TileType.MachineFeederRight: return TileType.MachineFeederDown;
			case TileType.MachineFeederDown: return TileType.MachineFeederLeft;
			case TileType.MachineFeederLeft: return TileType.MachineFeederUp;
		}
		return null;
	}
}
