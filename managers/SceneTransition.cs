using Godot;
using System;

public class SceneTransition : CanvasLayer
{
    private string _path;
    private AnimationPlayer _animationPlayer;


    public override void _Ready()
    {
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
}
