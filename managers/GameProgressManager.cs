using Godot;
using System;

public class GameProgressManager : Node2D
{
  [Signal] public delegate void game_won();

  public bool GameWon = false;
  public bool LevelWon = false;
  public int CurrentLevel = 0;
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
    LevelWon = false;
    NbItemGathered = 0;
    NbItemToWin = nbItemToWin;
    TypeOfItemToWin = typeOfItemToWin;
  }

  public void IncrNbItemGathered()
  {
    NbItemGathered++;
    if (!LevelWon && NbItemGathered >= NbItemToWin)
    {
      LevelWon = true;
      EmitSignal(nameof(game_won));
    }
  }

  public void GoToNextLevel()
  {
    LevelWon = false;
    CurrentLevel++;

    if (CurrentLevel == Lastlevel)
    {
      GameWon = true;
      _sceneTransition.FadeTo("Win.tscn");
      return;
    }
    _mapManager.InitLevel(CurrentLevel);
  }
}