using Godot;
using System;

public class InGameTransition : Control
{
    private GameProgressManager _gameProgressManager;
    private AnimationPlayer _animationPlayer;
    private TextureRect _textureRect;

    public override void _Ready()
    {
        // Autoloads
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton

        _animationPlayer = GetNode<AnimationPlayer>("ColorRect/AnimationPlayer");
        _textureRect = GetNode<TextureRect>("ColorRect");


        _gameProgressManager.Connect("game_won", this, nameof(OnGameWon));
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == (int)ButtonList.Left && mb.Pressed
                && (float)(_textureRect.Material as ShaderMaterial).GetShaderParam("progress") >= 1f) // Hacky but hey
            {
                OnClick();
            }
        }
    }

    public void OnGameWon()
    {
        _animationPlayer.Play("FadeIn");
    }

    public void OnClick()
    {
        _animationPlayer.Play("FadeOut");
    }


    public void GoToNextLevel()
    {
        _gameProgressManager.GoToNextLevel();
    }
}
