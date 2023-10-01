using Godot;
using System;

public class MapTileMap : TileMap
{
    private TileBuilderManager _tileBuilderManager;
    private MapManager _mapManager;
    private GameProgressManager _gameProgressManager;

    public override void _Ready()
    {
        // Autoloads
        _tileBuilderManager = (TileBuilderManager)GetNode($"/root/{nameof(TileBuilderManager)}"); // Singleton
        _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
    }

    public override void _Input(InputEvent evt)
    {
        if (evt is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
        {
            if (_gameProgressManager.InputStarted)
                return;

            Vector2i tilePos = WorldToMap(GetGlobalMousePosition()) * 0.5f;

            if (eventMouseButton.ButtonIndex == (int)ButtonList.Left)
            {
                if (_mapManager.TryGetTileType(tilePos, out _))
                {
                    _mapManager.RotateMachine(tilePos);
                }
                else
                {
                    _tileBuilderManager.PlaceMachine(tilePos);
                }
            }
            else if (eventMouseButton.ButtonIndex == (int)ButtonList.Right)
            {
                _tileBuilderManager.DestroyMachine(tilePos);
            }
        }
        else if (evt is InputEventMouseMotion eventMouseMotion)
        {
            if (_gameProgressManager.InputStarted)
            {
                // Only show highlight in build mode
                return;
            }

            Vector2i tilePos = WorldToMap(GetGlobalMousePosition()) * 0.5f;
            _tileBuilderManager.HighlightPotentialMachine(tilePos);
        }
    }
}
