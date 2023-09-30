using Godot;
using System;

public class SceneTransition : CanvasLayer
{
    private string _path;
    private GameProgressManager _gameProgressManager;

    private AnimationPlayer _animationPlayer;


    public override void _Ready()
    {
        // Autoloads
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    // PUBLIC FUNCTION. CALLED WHENEVER YOU WANT TO CHANGE SCENE
    public void FadeTo(string scenePath)
    {
        _path = scenePath;
        _animationPlayer.Play("Fade");
    }

    // PRIVATE FUNCTION. CALLED AT THE MIDDLE OF THE TRANSITION ANIMATION
    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(_path))
            GetTree().ChangeScene(_path);

    }

    private void UpdateMapABitAfterChangeScene()
    {
        // hacky as fuck
        if (_path == "Main.tscn")
            _gameProgressManager.GoToNextLevel();
    }
}
