using Godot;
using System;

public class StartStopButton : TextureButton
{
    private GameProgressManager _gameProgressManager;
    private SoundManager _soundManager;
    private Texture _startButtonTexture;
    private Texture _stopButtonTexture;
    private Texture _resetButtonTexture;

    private TextureRect _textureRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Autoloads
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton

        _textureRect = GetNode<TextureRect>("TextureRect");

        _startButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/start-button.png");
        _stopButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/stop-button.png");
        _resetButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/reset-button.png");

        _gameProgressManager.Connect("input_started_changed", this, nameof(InputStartedChanged));
    }

    public void OnClick()
    {
        if (!_gameProgressManager.InputStarted)
        {
            _gameProgressManager.StartInputs();
        }
        else
        {
            _gameProgressManager.StopInputsResetItem();
        }
        _soundManager.PlayClick();
    }

    public void InputStartedChanged()
    {
        if (_gameProgressManager.InputStarted)
        {
            _textureRect.Texture = _stopButtonTexture;
        }
        else
        {
            _textureRect.Texture = _startButtonTexture;
        }
    }

}
