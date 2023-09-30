using Godot;
using System;

public class GameProgressManager : Node2D
{
  public int CurrentLevel = -1;
  public int Lastlevel = 5;

  private SceneTransition _sceneTransition;
  private MapManager _mapManager;

  public int NbItemGathered;
  public int NbItemToWin { get; private set; }
  public PigPerks TypeOfItemToWin { get; private set; }

  public override void _Ready()
  {
    // Autoloads
    _sceneTransition = (SceneTransition)GetNode($"/root/{nameof(SceneTransition)}"); // Singleton
    _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton
  }

  public void Init(int nbItemToWin, PigPerks typeOfItemToWin)
  {
    NbItemGathered = 0;
    NbItemToWin = nbItemToWin;
    TypeOfItemToWin = typeOfItemToWin;
  }

  public void CheckIfWon()
  {
    if (NbItemGathered >= NbItemToWin)
      _sceneTransition.FadeTo("BetweenLevel.tscn");
  }

  public void GoToNextLevel()
  {
    CurrentLevel++;

    if (CurrentLevel == Lastlevel)
    {
      GD.Print("Victory: TODO transition to winning screen");
      return;
    }

    _mapManager.InitLevel(CurrentLevel);
  }
}