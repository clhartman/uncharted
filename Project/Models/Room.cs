using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<string, IRoom> Exits { get; set; }


    public void AddRoom(string direction, IRoom room)
    {
      Exits.Add(direction, room);
    }

    public void AddItem(Item item)
    {
      Items.Add(item);
    }

    public IRoom Go(string direction)
    {
      if (Exits.ContainsKey(direction))
      {
        return Exits[direction];
      }
      Console.WriteLine("You can't go that way.");
      return this;
    }

    public Item TakeItem(string itemName)
    {
      System.Console.WriteLine($"Searching in {Name} for {itemName}");
      foreach (Item item in Items)
      {
        if (item.Name.ToLower() == itemName.ToLower())
        {
          Items.Remove(item);
          System.Console.WriteLine($"Adding {itemName} to inventory");
          return item;
        }
      }
      System.Console.WriteLine($"It doesn't seem like {itemName} is in {Name}");
      return null;
    }

    public void Print()
    {
      Console.WriteLine($"You are in {Name}. {Description}");
      Console.WriteLine("What would you like to do?");

    }


    public Room(string name, string description)
    {
      Name = name;
      Description = description;
      Exits = new Dictionary<string, IRoom>();
      Items = new List<Item>();
    }
  }
}