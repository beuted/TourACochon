using Godot;
using System;

public class MapTileMap : TileMap
{
    private TileBuilderManager _tileBuilderManager;

    public override void _Ready()
    {
        // Autoloads
        _tileBuilderManager = (TileBuilderManager)GetNode($"/root/{nameof(TileBuilderManager)}"); // Singleton
    }

    public override void _Input(InputEvent evt)
    {
        if (evt is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == (int)ButtonList.Left)
        {
            var tilePos = WorldToMap(eventMouseButton.Position);

            _tileBuilderManager.PlaceMachine(new Vector2i((int)tilePos.x, (int)tilePos.y));
        }
    }
}
