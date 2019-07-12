using System;
using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Item : IItem, IEquatable<Item>, IComparer<Item>
  {
    public string Name { get; set; }
    public string Description { get; set; }


    //should the add items method be on this page, or on the room page if the person can only collect the item in a specific room?
    public Item(string name, string description)
    {
      Name = name;
      Description = description;
    }

    public bool Equals(Item other)
    {

      // System.Console.WriteLine($"ITEM - Equals - {this.ToString()} & { other.ToString() }");

      System.Console.WriteLine($"ITEM - Equals to lower - this.Name {this.Name.ToLower()} & { other.Name.ToLower() }");
      System.Console.WriteLine($"ITEM - Equals == {this.Name.ToLower().Equals(other.Name.ToLower()) }");
      return this.Name.ToLower().Equals(other.Name.ToLower());
    }

    public override bool Equals(Object obj)
    {

      // System.Console.WriteLine($"ITEM - override Equals - {this.ToString()} & { ((Item)obj).ToString() }");
      return this.Equals((Item)obj);
    }

    public override string ToString()
    {
      return $"Item -> Name: {Name}, Description: {Description}";
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }

    public int Compare(Item x, Item y)
    {
      System.Console.WriteLine($"ITEM - Compare to lower - this.Name {x.Name.ToLower()} & { y.Name.ToLower() }");
      System.Console.WriteLine($"ITEM - Compare - compareto {x.Name.ToLower().CompareTo(y.Name.ToLower()) }");
      return x.Name.ToLower().CompareTo(y.Name.ToLower());
    }
  }
}