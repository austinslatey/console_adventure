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
  private Location startingRoom;
  private Location treasureRoom;
  private string playerName;
  private int playerHealth;
  private int guardianHealth;
  private int playerAttackPower;
  private int guardianAttackPower;
  private List<string> playerInventory;
  private bool potionAcquired = false;
  private bool armorAcquired = false;

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

    // Initialize player inventory
    playerInventory = new List<string>();
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
    startingRoom = new Location("Starting Room", $"Hello {playerName}! Brave and curious explorer! \n\nType: Go/Move `North, East, South, West` to find the hidden treasure.");
    currentRoom = startingRoom;

    // Create rooms for the different directions
    Location roomNorth = new Location("Room North", "The air smells of burnt wood and sulfur...");
    Location roomEast = new Location("Room East", @"
  You find a potion! This may be useful later
        .
    .
      O  o
     .

  .  O
       .
     o
    o   .
  _________
c(`       ')o
  \.     ,/
 _//^---^\\_ 
");
    Location gaurdianRoom = new Location("Room South", "Oh no The Guardian!!! A powerful and ancient creature that protects the city's most valuable treasure. Beat the monster hero.");
    // Initialize treasureRoom
    this.treasureRoom = new Location("Treasure Room", @"
        _______          
      .'_/_|_\_'.      
      \'\ -|- /'/
       `\\-|-//'
         `\|/`
           '
You enter a chamber filled with strange artifacts and mysterious symbols carved into the walls. The air crackles with an otherworldly energy, sending shivers down your spine.");

    Location armorRoom = new Location("Armor Room", "You find some old armor that seems to be abandoned. You gear up for the adventure still awaits!");
    // Connect rooms in all directions from the starting room
    startingRoom.Exits["north"] = roomNorth;
    startingRoom.Exits["east"] = roomEast;
    startingRoom.Exits["south"] = armorRoom;

    // Connect back to the starting room from all other rooms
    roomNorth.Exits["south"] = startingRoom;
    roomNorth.Exits["east"] = gaurdianRoom;

    roomEast.Exits["west"] = startingRoom;
    gaurdianRoom.Exits["north"] = startingRoom;

    treasureRoom.Exits["south"] = startingRoom;
    armorRoom.Exits["north"] = startingRoom;

    // Set the starting room
    currentRoom = startingRoom;
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

      // Check if the player has moved to the room containing the Oracle
      if (currentRoom.Name == "Room North")
      {
        Console.WriteLine(@"
       /\
      /__\
     /____\
    /      \
   /  \__/  \
  /   @  @   \
 /   /\  /\   \
/ __/  \/  \__ \");
        Console.WriteLine("You find Oracle the Wise One! Listen, for he may have clues");
        Console.WriteLine("Would you like to listen to Oracle? (yes/no)");
        string choice = Console.ReadLine().ToLower();

        // Process the player's choice to listen to Oracle
        if (choice == "yes")
        {
          Console.WriteLine("---------\nOracle: The roars out east.. oh so mighty I do say. Treasure beyond I forsee.");
        }
        else if (choice == "no")
        {
          Console.WriteLine("The wise one gazes into the distance...");
        }
        else
        {
          Console.WriteLine("Invalid choice. Try again.");
        }
      }
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
      Location nextRoom = currentRoom.Exits[direction];

      // Check if the next room is an item room and if the item has been acquired
      if ((nextRoom.Name == "Room East" && potionAcquired) ||
          (nextRoom.Name == "Armor Room" && armorAcquired))
      {
        Console.WriteLine("You have already acquired the item in this room.");
        return;
      }
      if (direction == "east" && nextRoom.Name == "Room East")
      {
        // Add the potion name or any identifier to the player's inventory
        playerInventory.Add("Potion");
        potionAcquired = true;
      }
      else if (direction == "south" && nextRoom.Name == "Armor Room")
      {
        // Add armor to player's inventory
        playerInventory.Add("Armor");
        // Increase player's health by 50 points
        playerHealth += 50;
        armorAcquired = true;
        Console.WriteLine("You found some armor and equipped it. Your health increased by 50 points!");
      }


      // Move to the next room
      currentRoom = nextRoom;

      // Handle special events/interactions when entering certain rooms
      if (direction == "east" && currentRoom.Name == "Room South")
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
      // Check if the player has a potion
      if (playerInventory.Contains("Potion"))
      {
        Console.WriteLine("3. Use Potion");
      }
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
        case "3":
          // Player chooses to use potion
          UsePotion();
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
        currentRoom = treasureRoom;
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
        // Check if the player has a potion and their health is below or equal to 1 HP
        if (playerInventory.Contains("Potion") && playerHealth <= 1)
        {
          // Use the potion to restore health
          UsePotion();
          Console.WriteLine("You had to use your potion.");
          // Ensure the player's health is not below 0
          if (playerHealth < 0)
          {
            playerHealth = 0;
          }
          // Check if the player is still defeated after using the potion
          if (playerHealth <= 0)
          {
            Console.WriteLine("You have been defeated by the Guardian!");
            // Reset health
            playerHealth = 100;
            guardianHealth = 150;
            // Move the player back to the starting room
            currentRoom = startingRoom;
            // Print the starting room description
            PrintRoomDescription();
            return;
          }
        }
        else
        {
          Console.WriteLine("You have been defeated by the Guardian!");
          // Reset health
          playerHealth = 100;
          guardianHealth = 150;
          // Move the player back to the starting room
          currentRoom = startingRoom;
          // Print the starting room description
          PrintRoomDescription();
          return;
        }
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
    // Damage calculation logic here
    // For simplicity, let's assume a random damage within a certain range
    Random random = new Random();
    // 50% of attack power
    int minDamage = (int)(attackPower * 0.5);
    // 150% of attack power
    int maxDamage = (int)(attackPower * 1.5);
    return random.Next(minDamage, maxDamage + 1);
  }

  private void UsePotion()
  {
    // Increase player's health and remove the potion from inventory
    Console.WriteLine("You used a potion to restore health!");
    // Increase health by a certain amount, adjust as needed
    playerHealth += 50;
    playerInventory.Remove("Potion");
    Console.WriteLine($"Your health: {playerHealth}");
  }
}