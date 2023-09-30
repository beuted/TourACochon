using Godot;
using System;

public class Menu : Node
{
    private SceneTransition _sceneTransition;

    public override void _Ready()
    {
        // Autoloads
        _sceneTransition = (SceneTransition)GetNode($"/root/{nameof(SceneTransition)}"); // Singleton
    }


    public void OnClickPlay()
    {
        _sceneTransition.FadeTo("BetweenLevel.tscn");
    }

    public void OnClickExit()
    {
        GetTree().Quit();
    }
}
