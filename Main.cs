using Godot;
using System;

public class Main : Node
{
    private MapManager _mapManager;
    private CameraManager _cameraManager;
    private SoundManager _soundManager;


    public override void _Ready()
    {
        // Autoloads
        _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton
        _cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton

        _mapManager.Init(GetNode<TileMap>("TileMap"), GetNode<Node2D>("ItemsContainer"));
        _cameraManager.Init(GetNode<Camera>("Camera"));
        _soundManager.Init();

        // Start Music
        _soundManager.PlayMusic();
    }

}
