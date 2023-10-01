using Godot;
using System;

public class StartStopButton : TextureButton
{
    private GameProgressManager _gameProgressManager;

    private Texture _startButtonTexture;
    private Texture _stopButtonTexture;
    private Texture _resetButtonTexture;

    private TextureRect _textureRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");

        _startButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/start-button.png");
        _stopButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/stop-button.png");
        _resetButtonTexture = ResourceLoader.Load<Texture>("res://assets/graphics/reset-button.png");
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton
    }

    public void OnClick()
    {
        GD.Print("click");
        if (!_gameProgressManager.InputStarted)
        {
            _textureRect.Texture = _stopButtonTexture;
            _gameProgressManager.StartInputs();
        }
        else
        {
            _textureRect.Texture = _startButtonTexture;
            _gameProgressManager.StopInputsResetItem();
        }
    }

}
