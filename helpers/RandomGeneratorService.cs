using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public static class RandomGeneratorService
{
  public static RandomNumberGenerator Random = new RandomNumberGenerator();

  private static Random Rnd = new Random(/*System.Environment.TickCount*/ 42);

  static RandomGeneratorService()
  {
    //Random.Seed = 423;
    Random.Randomize();
    GD.Print("RandomGeneratorService initialized with seed: ", Random.Seed);
  }

  public static List<T> GetRandomFromList<T>(List<T> list, int nbPicks)
  {
    return list.OrderBy(x => Rnd.Next()).Take(nbPicks).ToList();
  }
}