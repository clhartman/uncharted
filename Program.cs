using System;
using Uncharted.Project;

namespace Uncharted
{
  public class Program
  {
    public static void Main(string[] args)
    {
      GameService game = new GameService();
      game.StartGame();
    }
  }
}
