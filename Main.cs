using Godot;
using System;

public class Main : Node
{
    private MapManager _mapManager;


    public override void _Ready()
    {
        // Autoloads
        _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton

        _mapManager.Init(GetNode<TileMap>("TileMap"), GetNode<Node2D>("ItemsContainer"));
    }

}
