using Godot;
using System;

public class ResetLevelButton : TextureButton
{
    private GameProgressManager _gameProgressManager;

    private TextureRect _textureRect;
    private TextureRect _textureRectStartStopButton;
    private Texture _startButtonTexture;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");

        _textureRectStartStopButton = GetNode<TextureRect>("../StartStopButton/TextureRect");

        _startButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/start-button.png");

        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
    }

    public void OnClick()
    {
        _gameProgressManager.ResetLevel();

        // Very hacky mais la flemme
        _textureRectStartStopButton.Texture = _startButtonTexture;
    }

}
