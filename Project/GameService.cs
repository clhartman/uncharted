using System;
using System.Collections.Generic;
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
        case "quit":
          Quit();
          break;
        default:
          return;
      }
    }

    public void Go(string direction)
    {
      Room destination = (Room)CurrentRoom.Go(direction);
      // if (destination == null)
      // {
      //   KillLoser();
      // }
      // else
      // {
      CurrentRoom = destination;
      // }
    }

    public void Help()
    {
      throw new System.NotImplementedException();
    }

    public void Inventory()
    {
      foreach (var item in CurrentPlayer.Inventory)
      {
        System.Console.WriteLine(item.Name);
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
      Room entry = new Room("entry", "A wide, square room");
      Room bridge = new Room("bridge", "A room with a rickety rope bridge over a deep chasm");
      Room tower = new Room("tower", "A circular room.");
      Room treasure = new Room("treasure", "A murky basement room. You see nothing, because it's dark.");
      #endregion

      #region items
      Item grappling = new Item("hook", "It is a metal hook attached to a long rope");
      Item torch = new Item("torch", "It's a torch. It casts light so you can see.");
      Item paperweight = new Item("paperweight", "It's a paperweight. It looks like it's made of glass, but it's quite heavy and is essentially worthless");
      Item goldStatue = new Item("statue", "It's a golden statue of El Dorado. It's large and heavy and it's amazing that you can lift it. It is decidedly NOT worthless");
      Item ropeBridge = new Item("bridge", "A rickety rope bridge. It does not look stable.");
      Item sconce = new Item("sconce", "A metal sconce. It looks like a torch would fit into it.");
      Item pedestal = new Item("pedestal", "A light shines on the pedestal. The effect is striking. You're speechless.");
      Item DEM = new Item("DEM", "Limitless Power!!!!");
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
      entry.AddItem(DEM);
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
      // Console.Clear();
      System.Console.WriteLine("Welcome to your adventure. What is your name?");
      string name = Console.ReadLine();
      CurrentPlayer = new Player(name);
      CurrentRoom = StartLocation;
      System.Console.WriteLine($"Thank you, '{name}'. However, you and I both know that you're ACTUALLY {name} Drake, long-lost descendant of Sir Francis Drake, wily adventurer, and you're ready to make your namesake proud. \r\nYou have killed A LOT of nameless mercenaries to get here on your quest for the statue of El Dorado. You stand in front of a non-descript wooden door. \nIt has markings that match drawings from your ancestor's diary, and you're pretty sure this is the spot. Do you enter? y/n: ");
      string choice = Console.ReadLine().ToLower();
      if (choice == "y")
      {
        while (Running)
        {
          CurrentRoom.Print();
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
    // public void KillLoser()
    // {
    //   string obit = CurrentRoom.DeathScene;
    //   System.Console.WriteLine(obit);
    //   Quit();
    // }

    public void UseItem(string itemName)
    {
      Item itemToUse = CurrentPlayer.Inventory.Find(item => item.Name.ToLower() == itemName.ToLower());
      //switch statement which sends to a method for specific item - that item then validates the room
      // if (CurrentPlayer.Inventory.Contains(itemToUse) && CurrentRoom.Name == "treasure")
      {
        switch (itemName)
        {
          case "torch":
            CurrentPlayer.Inventory.Remove(itemToUse);
            CurrentRoom.Items.Add(itemToUse);
            System.Console.WriteLine(@"The room is lit well, due to your torch. You see a pedestal in the middle of the room
            On the pedestal is a glimmering golden statue. It's El Dorado! The journal was right!");
            CurrentRoom.UseItem(itemName);
            break;
          // case "hook":
          //   CurrentRoom.UseItem(itemToUse);
          //   break;
          // case "paperweight":
          //   CurrentRoom.UseItem(itemToUse);
          //   break;
          // case "statue":
          //   CurrentRoom.UseItem(itemToUse);
          //   break;
          default:
            System.Console.WriteLine($"{itemName} is not a thing that exists here.");
            break;

        }
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