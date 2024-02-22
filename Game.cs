using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventureGame
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Welcome to the Console Adventure!");
      Console.Write("Please enter your name:  ");
      string playerName = Console.ReadLine();

      Game game = new Game(playerName);
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
  private string playerName;
  private int playerHealth;
  private int guardianHealth;
  private int playerAttackPower;
  private int guardianAttackPower;
  public Game(string playerName)
  {
    this.playerName = playerName;

    // Set initial player health
    this.playerHealth = 100;

    // Set initial guardian health
    this.guardianHealth = 150;

    // Set initial player and guardian attack power
    // Adjust values as needed
    this.playerAttackPower = 20;
    this.guardianAttackPower = 25;
  }
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
    Location startingRoom = new Location("Starting Room", $"Hello {playerName}! Brave and curious explorer! \n\nType: Go/Move `North, East, South, West` to find the hidden treasure.");
    Location currentRoom = startingRoom;

    string[] directions = { "north", "east", "south", "west" };

    foreach (string direction in directions)
    {
      Location nextRoom = new Location($"Room {direction.Substring(0, 1).ToUpper()}{direction.Substring(1)}", GetRoomDescription(direction));
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

  private string GetRoomDescription(string direction)
  {
    switch (direction)
    {
      case "north":
        return @"
       /\
      /__\
     /____\
    /      \
   /  \__/  \
  /   @  @   \
 /   /\  /\   \
/ __/  \/  \__ \

You find Oracle the Wise One! Listen, for he may have clues";
      case "east":
        return "You step into a dimly lit corridor lined with ancient tapestries. The air is heavy with the scent of musty parchment and decay.";
      case "south":
        return "Oh no The Guardian!!! A powerful and ancient creature that protects the city's most valuable treasure. Beat the monster hero.";
      case "west":
        return "You enter a chamber filled with strange artifacts and mysterious symbols carved into the walls. The air crackles with an otherworldly energy, sending shivers down your spine.";
      default:
        throw new ArgumentException("Invalid direction");
    }
  }
  private void PrintRoomDescription()
  {
    // Print room name and description
    Console.WriteLine("----------\nWelcome to the " + currentRoom.Name + ".\n");
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
      if (direction == "south")
      {
        DuelGuardian();
        // Exit the Move method to prevent printing room description after duel
        return;
      }
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

  private void DuelGuardian()
  {
    Console.WriteLine(@"
 ________________________________________
/ Hello, I am the Gaurdian. Fear my might.   \
\ This is my treasure!!                /
 ----------------------------------------
        \                    / \  //\
         \    |\___/|      /   \//  \\
              /0  0  \__  /    //  | \ \
             /     /  \/_/    //   |  \  \
             @_^_@'/   \/_   //    |   \   \
             //_^_/     \/_ //     |    \    \
          ( //) |        \///      |     \     \
        ( / /) _|_ /   )  //       |      \     _\
      ( // /) '/,_ _ _/  ( ; -.    |    _ _\.-~        .-~~~^-.
    (( / / )) ,-{        _      `-.|.-~-.           .~         `.
   (( // / ))  '/\      /                 ~-. _ .-~      .-~^-.  \
   (( /// ))      `.   {            }                   /      \  \
    (( / ))     .----~-.\        \-'                 .~         \  `. \^-.
               ///.----..>        \             _ -~             `.  ^-`  ^-_
                 ///-._ _ _ _ _ _ _}^ - - - - ~                     ~-- ,.-~
                                                                    /.-~
");
    // Implement the duel logic here
    Console.WriteLine("You have challenged the Guardian! Prepare for battle...");

    // Add more code here to handle the duel mechanics
    Console.WriteLine($"-------- \n{playerName}'s health: {playerHealth}    |    Gaurdian's health: {guardianHealth}");

    // Loop until the guardian or player is defeated
    while (guardianHealth > 0 && playerHealth > 0)
    {
      // Display options to the player
      Console.WriteLine("Choose your action:");
      Console.WriteLine("1. Attack");
      Console.WriteLine("2. Defend");
      Console.Write("Enter your choice: ");
      string choice = Console.ReadLine();

      // Process player's choice
      switch (choice)
      {
        case "1":
          // Player chooses to attack
          AttackGuardian();
          break;
        case "2":
          // Player chooses to defend
          Defend();
          break;
        default:
          Console.WriteLine("Invalid choice. Please enter 1 to attack or 2 to defend.");
          break;
      }

      // Check if the guardian is defeated
      if (guardianHealth <= 0)
      {
        Console.WriteLine("You have defeated the Guardian!");
        // Additional logic for winning the game or advancing to the next stage
        return;
      }

      // Guardian's turn (for simplicity, assume the guardian always attacks)
      int guardianDamage = CalculateDamage(guardianAttackPower);
      playerHealth -= guardianDamage;
      Console.WriteLine($"The Guardian attacked you for {guardianDamage} damage!");
      Console.WriteLine($"Your health: {playerHealth}");

      // Check if the player is defeated
      if (playerHealth <= 0)
      {
        Console.WriteLine("You have been defeated by the Guardian!");
        // Additional logic for losing the game or handling defeat
        return;
      }

    }
  }

  private void AttackGuardian()
  {
    // Calculate player's damage and deduct from guardian's health
    int playerDamage = CalculateDamage(playerAttackPower);
    guardianHealth -= playerDamage;
    Console.WriteLine($"You attacked the Guardian for {playerDamage} damage!");
    Console.WriteLine($"Guardian's health: {guardianHealth}");
  }

  private void Defend()
  {
    // For simplicity, you can reduce the damage taken by the player when defending
    int damageTaken = CalculateDamage(guardianAttackPower) / 2; // Half the damage
    playerHealth -= damageTaken;
    Console.WriteLine($"You defended against the Guardian's attack, but took {damageTaken} damage!");
    Console.WriteLine($"Your health: {playerHealth}");
  }

  private int CalculateDamage(int attackPower)
  {
    // Implement your damage calculation logic here
    // For simplicity, let's assume a random damage within a certain range
    Random random = new Random();
    int minDamage = (int)(attackPower * 0.5); // 50% of attack power
    int maxDamage = (int)(attackPower * 1.5); // 150% of attack power
    return random.Next(minDamage, maxDamage + 1);
  }
}