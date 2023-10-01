using Godot;
using System;

public class InGameTransition : Control
{
    private GameProgressManager _gameProgressManager;
    private AnimationPlayer _animationPlayer;
    private ColorRect _colorRect;

    public override void _Ready()
    {
        // Autoloads
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _colorRect = GetNode<ColorRect>("ColorRect");


        _gameProgressManager.Connect("game_won", this, nameof(OnGameWon));
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == (int)ButtonList.Left && mb.Pressed
                && _colorRect.Modulate.a >= 1f) // Hacky but hey
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
