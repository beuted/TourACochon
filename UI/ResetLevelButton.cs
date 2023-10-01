using Godot;
using System;

public class ResetLevelButton : TextureButton
{
    private GameProgressManager _gameProgressManager;


    private TextureRect _textureRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");

        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
    }

    public void OnClick()
    {
        _gameProgressManager.ResetLevel();
    }

}
