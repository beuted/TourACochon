using Godot;
using System;

public class Menu : Node
{
    private SceneTransition _sceneTransition;
    private SoundManager _soundManager;

    public override void _Ready()
    {
        // Autoloads
        _sceneTransition = (SceneTransition)GetNode($"/root/{nameof(SceneTransition)}"); // Singleton
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton

        _soundManager.Init();

        // Start Music
        _soundManager.PlayMusic();
    }


    public void OnClickPlay()
    {
        _sceneTransition.FadeTo("Main.tscn");
        _soundManager.PlayClick();
    }

    public void OnClickExit()
    {
        GetTree().Quit();
    }
}
