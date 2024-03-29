using System;
using System.Collections.Generic;
using System.Threading;
using Uncharted.Project.Interfaces;
using Uncharted.Project.Models;

namespace Uncharted.Project
{
  public class GameService : IGameService
  {
    public Room CurrentRoom { get; set; }
    public Room StartLocation { get; set; }
    public Player CurrentPlayer { get; set; }
    private bool Running = true;



    public void GetUserInput()
    {
      string input = Console.ReadLine().ToLower();
      string[] inputs = input.Split(' '); //tokenize the string 
      string command = inputs[0];
      string option = "";
      if (inputs.Length > 1)
      {
        option = inputs[inputs.Length - 1];
      }
      switch (command)
      {
        case "go":
          Go(option);
          break;
        case "take":
          TakeItem(option);
          break;
        case "use":
        case "place":
          UseItem(option);
          break;
        case "break":
        case "open":
        case "swing":
        case "platform":
        case "look":
          Look();
          break;
        case "inventory":
          Inventory();
          break;
        case "help":
          Help();
          break;
        case "quit":
          Quit();
          break;
        default:
          return;
      }
    }

    public void Go(string direction)
    {
      Console.Clear();
      Room destination = (Room)CurrentRoom.Go(direction);
      // if (CurrentRoom.Name == "tower" && direction == "down")
      // {
      //   CurrentRoom.TowerDrop();
      // }
      // else
      {
        CurrentRoom = destination;
        CurrentRoom.Print();
      }
    }

    public void Help()
    {
      Console.Clear();
      System.Console.WriteLine(@"Choices:
      'go' : choose a direction to move
      'take' : to take an item from the room
      'use' : to use the item in your inventory
      'look' : to remind you about the room you're currently in
      'help' : for instructions on what you can do
      'inventory' : to see what items are currently in your inventory
      'quit' : to end the current game");
    }

    public void Inventory()
    {
      foreach (var item in CurrentPlayer.Inventory)
      {
        System.Console.WriteLine($"{item.Name} is in your inventory");
      }
      if (CurrentPlayer.Inventory.Count == 0)
      {
        System.Console.WriteLine("Your inventory is empty.");
      }
    }

    public void Look()
    {
      Console.Clear();
      Console.WriteLine(CurrentRoom.Description);
    }

    public void Quit()
    {
      Console.WriteLine("Would you like to try again? y/n: ");
      string response = Console.ReadLine().ToLower();
      Console.Clear();
      if (response == "y")
      {
        Console.Clear();
        Reset();
      }
      if (response == "n")
      {
        Running = false;
        System.Console.WriteLine("Thank you for playing!");
        Thread.Sleep(2000);
        Console.Clear();
        Environment.Exit(0);
      }
      else
      {
        Console.WriteLine("That's not a real choice.");
        Quit();
      }
    }

    public void Reset()
    {
      Setup();
      StartGame();
    }

    public void Setup()
    {
      #region rooms
      Room entry = new Room("entry", @"It's a wide, square room, with a lit torch resting comfortably in a sconce next to you. 
      To the north is a door.");
      Room bridge = new Room("bridge", @"It's a room - you're guessing. 
      You can't see anything, because it's dark.
      You know the entry room is still to the south, but beyond that, you're essentially blind.");
      Room tower = new Room("tower", @"You are in a circular room, with a set of winding stairs leading down.");
      Room treasure = new Room("treasure", "A murky basement room. You see nothing, because it's dark.");
      #endregion

      #region items
      Item grappling = new Item("hook", "It is a metal hook attached to a long rope.");
      Item torch = new Item("torch", "It's a torch. It casts light so you can see.");
      Item paperweight = new Item("paperweight", "It's a paperweight. It looks like it's made of glass, but it's quite heavy and is essentially worthless");
      Item goldStatue = new Item("statue", "It's a golden statue of El Dorado. It's large and heavy and it's amazing that you can lift it. It is decidedly NOT worthless");
      Item ropeBridge = new Item("bridge", "A rickety rope bridge. It does not look stable.");
      Item sconce = new Item("sconce", "A metal sconce. It looks like a torch would fit into it.");
      Item pedestal = new Item("pedestal", "A light shines on the pedestal. The effect is striking. You're speechless.");
      #endregion

      #region relationships
      entry.Exits.Add("north", bridge);
      bridge.Exits.Add("south", entry);
      bridge.Exits.Add("north", tower);
      // (string direction) =>
      // {
      //   if (bridge.Items.Contains(DEM))
      //   {
      //     return tower;
      //   }
      //   else
      //   {
      //     bridge.DeathScene = "Rickety bridges are an embarrassing data type conversion. You plummet to your death. But not immediate death. It's prolongued and excruciating.";
      //     return null;
      //   }
      // });
      // bridge.AddUsableItem(DEM, (Item itemToUse) =>
      // {
      //   bridge.AddItem(itemToUse);
      //   return "";
      // });
      // tower.AddUsableItem(torch, (Item itemToUse) =>
      // {
      //   tower.AddItem(itemToUse);
      //   return "";
      // });
      tower.Exits.Add("south", bridge);
      tower.Exits.Add("down", treasure);
      entry.AddItem(grappling);
      entry.AddItem(torch);
      entry.AddItem(paperweight);
      entry.AddItem(sconce);
      bridge.AddItem(ropeBridge);
      bridge.AddItem(sconce);
      tower.AddItem(sconce);
      treasure.AddItem(goldStatue);
      treasure.AddItem(pedestal);
      #endregion

      StartLocation = entry;

    }

    public void StartGame()
    {
      Console.Clear();
      System.Console.WriteLine("Welcome to your adventure. What is your name?");
      string name = Console.ReadLine();
      CurrentPlayer = new Player(name);
      CurrentRoom = StartLocation;
      Console.Clear();
      System.Console.WriteLine($@"Thank you, '{name}'. 
      However, you and I both know that you're ACTUALLY Nathan Drake, long-lost descendant of Sir Francis Drake,
      wily adventurer, and you're ready to make your namesake proud.
      You have killed A LOT of nameless mercenaries to get here on your quest for the statue of El Dorado. 
      You stand in front of a non-descript wooden door. 
      It has markings that match drawings from your ancestor's diary, and you're pretty sure this is the spot. 
      Do you enter? y/n: ");
      {
        string choice = Console.ReadLine().ToLower();
        if (choice == "y")
        {
          Console.Clear();
          System.Console.WriteLine(@"At any time, type 
          'go' to move in a specific direction
          'take' to take something from the room
          'use' to use an item that is in your inventory
          'help' to see your options 
          'inventory' to see what you currently have in your inventory
          'quit' to end the game");
          Thread.Sleep(8000);
          Console.Clear();
          CurrentRoom.Print();
          while (Running)
          {
            GetUserInput();
          }
        }
        if (choice == "n")
        {
          Console.Clear();
          Console.WriteLine("An unkilled nameless mercenary was hiding behind a burned-out vehicle. In your moment of indecision, he headshots you.");
          Quit();
        }
        Console.WriteLine("Thanks for playing!");
      }
    }
    public void TakeItem(string itemName)
    {
      Item itemTaken = CurrentRoom.Items.Find(item => item.Name.ToLower() == itemName.ToLower());
      if (itemName == "statue")
      {
        Win();
      }
      else if (itemTaken != null)
      {
        CurrentRoom.Items.Remove(itemTaken);
        CurrentPlayer.Inventory.Add(itemTaken);
        System.Console.WriteLine($"You have added {itemTaken.Name} to your inventory. {itemTaken.Description}");
      }
      else
      {
        System.Console.WriteLine("Invalid Option");
      }
    }
    public void UseItem(string itemName)
    {
      Item itemToUse = CurrentPlayer.Inventory.Find(item => item.Name.ToLower() == itemName.ToLower());
      //switch statement which sends to a method for specific item - that item then validates the room
      if (CurrentPlayer.Inventory.Contains(itemToUse))
      {
        switch (itemToUse.Name)
        {
          case "torch":
            // CurrentPlayer.Inventory.Remove(itemToUse);
            CurrentRoom.Items.Add(itemToUse);
            CurrentRoom.UseItem(itemName);
            break;
          case "hook":
            CurrentPlayer.Inventory.Remove(itemToUse);
            CurrentRoom.Items.Add(itemToUse);
            CurrentRoom.UseItem(itemName);
            break;
          // case "paperweight":
          //   CurrentRoom.UseItem(itemToUse);
          //   break;
          // case "statue":
          //   CurrentRoom.UseItem(itemToUse);
          //   break;
          default:
            System.Console.WriteLine($"{itemName} is not in your inventory.");
            break;

        }


      }
      else
      {
        System.Console.WriteLine($"{itemName} is not in your inventory");
      }
      // else
      // {
      //   System.Console.WriteLine($"You don't seem to have {itemName} in your inventory. Perhaps you should remedy that?");
      // }
    }

    public void Win()
    {
      System.Console.WriteLine("You quickly grab the statue from the pedestal. You pocket it, and begin to search for an exit. You push open the door and make your way up the mossy stairs, to the light. Sully is waiting for you, and you board his plane and fly off. You actually retrieved a priceless treasure, and no cities were unceremoniously destroyed. Shame about all the mercenaries you murdered, but hey. All in a day's work.");
      Quit();
    }

    public GameService()
    {
      Setup();
    }

  }
}