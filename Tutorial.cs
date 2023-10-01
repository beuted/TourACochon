using Godot;
using System;

public class Tutorial : Control
{
	private GameProgressManager _gameProgressManager;
	
    public override void _Ready()
    {
		_gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}");

		if (_gameProgressManager.CurrentLevel != 0)
		{
			this.Visible = false;
		}
    }
    
    public void OnClick()
    {
	    this.Visible = false;
    }
    
}
