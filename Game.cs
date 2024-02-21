using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventureGame
{
  class Program
  {
    static void Main(string[] args)
    {
      Game game = new Game();
      game.Start();
    }
  }
}


public class Location
{
  public string Name { get; }
  public string Description { get; }
  public Dictionary<string, Location> Exits { get; }

  public Location(string name, string description)
  {
    Name = name;
    Description = description;
    Exits = new Dictionary<string, Location>();
  }
}

public class Game
{
  // Declare currentRoom variable
  private Location currentRoom;
  public void Start()
  {
    // Initialize game
    Initialize();

    while (true)
    {
      // Print current room description
      PrintRoomDescription();

      // Get user input
      string input = GetUserInput();

      // Process user input
      ProcessInput(input);
    }
  }

  private void Initialize()
  {
    // Set up game data, rooms, items, etc.
    Location startingRoom = new Location("Starting Room", "Hello brave and curious explorer! \nType Go/Move `North, East, South, West` to find the hidden treasure.");
    Location currentRoom = startingRoom;

    string[] directions = { "north", "east", "south", "west" };

    foreach (string direction in directions)
    {
      Location nextRoom = new Location($"Room {direction.Substring(0, 1).ToUpper()}{direction.Substring(1)}", $"This is the {direction} room.");
      currentRoom.Exits[direction] = nextRoom;
      nextRoom.Exits[GetOppositeDirection(direction)] = currentRoom;
      currentRoom = nextRoom;
    }

    // Set the starting room
    this.currentRoom = startingRoom;
  }

  private string GetOppositeDirection(string direction)
  {
    switch (direction)
    {
      case "north": return "south";
      case "east": return "west";
      case "south": return "north";
      case "west": return "east";
      default: throw new ArgumentException("Invalid direction");
    }
  }
  private void PrintRoomDescription()
  {
    // Print room name and description
    Console.WriteLine("You are in the " + currentRoom.Name + ".");
    Console.WriteLine(currentRoom.Description);

    // Print exits
    Console.WriteLine("Exits: " + string.Join(", ", currentRoom.Exits.Keys));
  }

  private string GetUserInput()
  {
    Console.Write("> ");
    return Console.ReadLine();
  }

  private void ProcessInput(string input)
  {
    // Handle user input (move, interact with objects, etc.)
    if (input.StartsWith("go ") || input.StartsWith("move "))
    {
      string direction = input.Split(' ')[1];
      Move(direction);
    }
    else
    {
      Console.WriteLine("Invalid command. Try again.");
    }
  }

  private void Move(string direction)
  {
    // Check if the specified direction is a valid exit
    if (currentRoom.Exits.ContainsKey(direction))
    {
      // Move to the next room
      currentRoom = currentRoom.Exits[direction];
    }
    else
    {
      Console.WriteLine("You cannot go in that direction.");
    }
  }

  public class Room
  {
    public string Name { get; }
    public string Description { get; }
    public Exit[] Exits { get; }

    public Room(string name, string description, Exit[] exits)
    {
      Name = name;
      Description = description;
      Exits = exits;
    }
  }

  public class Exit
  {
    public string Direction { get; }
    public Room TargetRoom { get; }

    public Exit(string direction, Room targetRoom)
    {
      Direction = direction;
      TargetRoom = targetRoom;
    }
  }
}