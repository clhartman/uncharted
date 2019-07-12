using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Item : IItem
  {
    public string Name { get; set; }
    public string Description { get; set; }


    //should the add items method be on this page, or on the room page if the person can only collect the item in a specific room?
    public Item(string name, string description)
    {
      Name = name;
      Description = description;
    }
  }
}