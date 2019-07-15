using System;
using System.Collections.Generic;
using Uncharted.Project.Interfaces;

namespace Uncharted.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<string, IRoom> Exits { get; set; }

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

    public Item TakeItem(Item item) //see if this works
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

    public void UseItem(string itemToUse)
    {
      System.Console.WriteLine($"You have used the {itemToUse} in the {Name} room.");
      if (Name == "treasure" && itemToUse == "torch")
      {
        Description = @"The room is lit well, due to your torch. You see a pedestal in the middle of the room
            On the pedestal is a glimmering golden statue. It's El Dorado! The journal was right! What do you do?";
      }
      else if (Name == "bridge" && itemToUse == "torch")
      {
        Description = @"It's a room with a rickety rope bridge over a deep chasm. 
        The bridge does not look particularly safe, but you don't see any other options.
        Over the bridge, to the north, is an open archway. What do you do?";
      }
      else if (Name == "tower" && itemToUse == "hook")
      {
        System.Console.WriteLine(@"You swing the hook and rope combo in a circular motion.
        You let the hook fly up towards the ring in the ceiling, and it connects!
        You hold onto the rope, and swing out over the chasm, and the hook stays firmly in place.
        The only way to go is down.
        What do you do?");
      }
      else
      {
        System.Console.WriteLine("I don't think that will work here.");
      }
      System.Console.WriteLine(Description);
      // Item usingItem =
      // foreach (var item in UsableItems.Keys)
      //   {

      //     System.Console.WriteLine(item.ToString());
      //   }
      //   if (UsableItems.ContainsKey(itemToUse))
      //   {
      //     UsableItems[itemToUse](itemToUse);
      //   }
    }

    // public void TowerDrop()
    // {
    //   Item hookCheck = Items.Find(item => item.Name.ToLower() == "hook");
    //   if (Items.Contains(Item hook))
    //   {
    //     System.Console.WriteLine("You make your way slowly down the rope");
    //   }
    //   System.Console.WriteLine("You're in the tower drop function");
    // }




    public void Print()
    {
      Console.WriteLine($"You are in a room. {Description}");
      Console.WriteLine("What would you like to do?");
    }


    public Room(string name, string description)
    {
      Name = name;
      Description = description;
      Exits = new Dictionary<string, IRoom>();
      // UsableItems = new Dictionary<Item, Func<Item, string>>();
      Items = new List<Item>();
    }
  }
}