using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project
{
  public class GameService : IGameService
  {
    public Room currentRoom { get; set; }
    public Room startLocation { get; set; }
    public Player CurrentPlayer { get; set; }
    private bool running = true;



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
        case "help":
        case "quit":
          Quit();
          break;
        default:
          return;
      }
    }

    public void Go(string direction)
    {
      Room destination = (Room)currentRoom.Go(direction);
      if (destination == null)
      {
        KillLoser();
      }
      else
      {
        currentRoom = destination;
      }
    }

    public void Help()
    {
      throw new System.NotImplementedException();
    }

    public void Inventory()
    {
      throw new System.NotImplementedException();
    }

    public void Look()
    {
      Console.WriteLine($"{currentRoom.Description}");
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
        running = false;
      }
      else
      {
        Console.WriteLine("That's not a real choice.");
        Quit();
      }
    }

    public void Reset()
    {
      StartGame();
    }

    public void Setup()
    {
      #region rooms
      Room entry = new Room("Entry", "A wide, square room");
      Room bridge = new Room("Bridge", "A room with a rickety rope bridge over a deep chasm");
      Room tower = new Room("Tower", "A circular room.");
      Room treasure = new Room("Treasure", "A murky basement room with a shining gold statue sitting on a pedestal");
      #endregion

      #region items
      Item grappling = new Item("Hook", "It is a metal hook attached to a long rope");
      Item torch = new Item("Torch", "It's a torch. It casts light so you can see.");
      Item paperweight = new Item("Paperweight", "It's a paperweight. It looks like it's made of glass, but it's quite heavy and is essentially worthless");
      Item goldStatue = new Item("Statue", "It's a golden statue of El Dorado. It's large and heavy and it's amazing that you can lift it. It is decidedly NOT worthless");
      Item ropeBridge = new Item("Bridge", "A rickety rope bridge. It does not look stable.");
      Item sconce = new Item("Sconce", "A metal sconce. It looks like a torch would fit into it.");
      Item pedestal = new Item("Pedestal", "A light shines on the pedestal. The effect is striking. You're speechless.");
      Item DEM = new Item("DEM", "Limitless Power!!!!");
      #endregion

      #region relationships
      entry.AddExit("north", (string direction) => bridge);
      bridge.AddExit("south", (string direction) => entry);
      bridge.AddExit("north", (string direction) =>
      {
        if (bridge.Items.Contains(DEM))
        {
          return tower;
        }
        else
        {
          bridge.DeathScene = "Rickety bridges are an embarrassing data type conversion. You plummet to your death. But not immediate death. It's prolongued and excruciating.";
          return null;
        }
      });
      bridge.AddUsableItem(DEM, (Item itemToUse) =>
      {
        bridge.AddItem(itemToUse);
        return "";
      });
      tower.AddUsableItem(torch, (Item itemToUse) =>
      {
        tower.AddItem(itemToUse);
        return "";
      });
      tower.AddExit("south", (string direction) => bridge);
      tower.AddExit("down", (string direction) => treasure);
      entry.AddItem(grappling);
      entry.AddItem(torch);
      entry.AddItem(DEM);
      entry.AddItem(paperweight);
      entry.AddItem(sconce);
      bridge.AddItem(ropeBridge);
      bridge.AddItem(sconce);
      tower.AddItem(sconce);
      treasure.AddItem(goldStatue);
      treasure.AddItem(pedestal);
      #endregion

      startLocation = entry;

    }

    public void StartGame()
    {
      // Console.Clear();
      System.Console.WriteLine("Welcome to your adventure. What is your name?");
      string name = Console.ReadLine();
      CurrentPlayer = new Player(name);
      currentRoom = startLocation;
      System.Console.WriteLine($"Thank you, '{name}'. However, you and I both know that you're ACTUALLY {name} Drake, long-lost descendant of Sir Francis Drake, wily adventurer, and you're ready to make your namesake proud. \r\nYou have killed A LOT of nameless mercenaries to get here on your quest for the statue of El Dorado. You stand in front of a non-descript wooden door. \nIt has markings that match drawings from your ancestor's diary, and you're pretty sure this is the spot. Do you enter? y/n: ");
      string choice = Console.ReadLine().ToLower();
      if (choice == "y")
      {
        while (running)
        {
          currentRoom.Print();
          GetUserInput();
        }
      }
      if (choice == "n")
      {
        Console.WriteLine("An unkilled nameless mercenary was hiding behind a burned-out vehicle. In your moment of indecision, he headshots you.");
        Quit();
      }
      Console.WriteLine("Thank you for playing!");
    }

    public void TakeItem(string itemName)
    {
      Item itemTaken = currentRoom.TakeItem(new Item(itemName, ""));
      if (itemTaken != null)
      {
        CurrentPlayer.Inventory.Add(itemTaken);
        System.Console.WriteLine($"You have added {itemTaken.Name} to your inventory. {itemTaken.Description}");
      }
    }
    public void KillLoser()
    {
      string obit = currentRoom.DeathScene;
      System.Console.WriteLine(obit);
      Quit();
    }

    public void UseItem(string itemName)
    {
      Item itemToUse = new Item(itemName, "");
      if (CurrentPlayer.Inventory.Contains(itemToUse))
      {
        currentRoom.UseItem(itemToUse);
      }
      else
      {
        System.Console.WriteLine($"You don't seem to have {itemName} in your inventory. Perhaps you should remedy that?");
      }
    }

    public GameService()
    {
      Setup();
    }

  }
}