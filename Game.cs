using System;
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

public class Game
{
  // Declare currentRoom variable
  private Room currentRoom;
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
    currentRoom = new Room("Starting Room", "This is the starting room.", new Exit[] {/* Define Exits */});
  }

  private void PrintRoomDescription()
  {
    // Print room name and description
    Console.WriteLine("You are in the " + currentRoom.Name + ".");
    Console.WriteLine(currentRoom.Description);

    // Print exits
    Console.WriteLine("Exits: " + string.Join(", ", currentRoom.Exits.Select(e => e.Direction)));
  }

  private string GetUserInput()
  {
    Console.Write("> ");
    return Console.ReadLine();
  }

  private void ProcessInput(string input)
  {
    // Handle user input (move, interact with objects, etc.)
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