using System.Collections.Generic;
using Uncharted.Project.Models;

namespace Uncharted.Project.Interfaces
{
  public interface IPlayer
  {
    string PlayerName { get; set; }
    List<Item> Inventory { get; set; }
  }
}
