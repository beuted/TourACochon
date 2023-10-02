using Godot;
using System;

public class ResetLevelButton : TextureButton
{
    private GameProgressManager _gameProgressManager;
    private SoundManager _soundManager;
    private TextureRect _textureRect;
    private TextureRect _textureRectStartStopButton;
    private Texture _startButtonTexture;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Autoloads
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton

        _textureRect = GetNode<TextureRect>("TextureRect");

        _textureRectStartStopButton = GetNode<TextureRect>("../StartStopButton/TextureRect");

        _startButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/start-button.png");
    }

    public void OnClick()
    {
        _gameProgressManager.ResetLevel();

        _soundManager.PlayClick();

        // Very hacky mais la flemme
        _textureRectStartStopButton.Texture = _startButtonTexture;
    }

}
