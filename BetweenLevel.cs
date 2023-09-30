using Godot;
using System;

public class BetweenLevel : Control
{
    private SceneTransition _sceneTransition;

    public override void _Ready()
    {
        // Autoloads
        _sceneTransition = (SceneTransition)GetNode($"/root/{nameof(SceneTransition)}"); // Singleton
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == (int)ButtonList.Left && mb.Pressed)
            {
                OnClick();
            }
        }
    }


    private void OnClick()
    {
        _sceneTransition.FadeTo("Main.tscn");
    }
}
