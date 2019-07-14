using System;
using System.Collections.Generic;
using Uncharted.Project.Interfaces;

namespace Uncharted.Project.Models
{
  public class Player : IPlayer
  {
    public string PlayerName { get; set; }
    public List<Item> Inventory { get; set; }


    public void AddItem(Item item)
    {
      Inventory.Add(item);
    }
    public Player(string name)
    {
      PlayerName = name;
      Inventory = new List<Item>();
    }
  }
}