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
    Location startingRoom = new Location("Starting Room", "This is the starting room.");
    Location secondRoom = new Location("Second Room", "This is the south room.");
    Location thirdRoom = new Location("Third Room", "This is the east room.");
    Location fourthRoom = new Location("Third Room", "This is the west room.");

    // Define exits for each room
    startingRoom.Exits["north"] = secondRoom;
    startingRoom.Exits["east"] = thirdRoom;
    startingRoom.Exits["west"] = fourthRoom;
    
    secondRoom.Exits["south"] = startingRoom;
    thirdRoom.Exits["west"] = startingRoom;
    fourthRoom.Exits["east"] = startingRoom;

    // Set the starting room
    currentRoom = startingRoom;
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