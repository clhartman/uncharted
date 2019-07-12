using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string DeathScene { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<Item, Func<Item, string>> UsableItems { get; set; }
    public Dictionary<string, Func<string, IRoom>> exits { get; set; }


    public void AddExit(string direction, Func<string, IRoom> fn)
    {
      exits.Add(direction, fn);
    }
    public void AddUsableItem(Item usableItem, Func<Item, string> fn)
    {
      UsableItems.Add(usableItem, fn);
    }

    public void AddItem(Item item)
    {
      Items.Add(item);
    }

    public IRoom Go(string direction)
    {
      if (exits.ContainsKey(direction))
      {
        return exits[direction](direction);
      }
      Console.WriteLine("You can't go that way.");
      return this;
    }

    public Item TakeItem(Item item)
    {
      System.Console.WriteLine($"Searching in {Name} for {item.Name}");
      Item foundItem = Items.Find((Item i) =>
      {
        // System.Console.WriteLine($"**** {i.Name} - {item.Name} -> {item.Equals(i)}");
        return item.Equals(i);
      });
      if (foundItem == null)
      {
        System.Console.WriteLine($"It doesn't seem like {item.Name} is in {Name}");
        return null;
      }
      else
      {
        Items.Remove(foundItem);
        System.Console.WriteLine($"Adding {item.Name} to inventory");
        return foundItem;
      }
    }

    public void UseItem(Item itemToUse)
    {

      foreach (var item in UsableItems.Keys)
      {

        System.Console.WriteLine(item.ToString());
      }
      if (UsableItems.ContainsKey(itemToUse))
      {
        UsableItems[itemToUse](itemToUse);
      }
      System.Console.WriteLine("I don't think that will work here.");
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
      exits = new Dictionary<string, Func<string, IRoom>>();
      UsableItems = new Dictionary<Item, Func<Item, string>>();
      Items = new List<Item>();
    }
  }
}