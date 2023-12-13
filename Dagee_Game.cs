using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using System.IO.Enumeration; // Use this for the sleep timer





class Program
{
    static void Main()
    {

        // Input the number of players
        Console.Write("Enter the number of players: ");
        int numPlayers = int.Parse(Console.ReadLine());



        // Create players
        List<Player> players = new List<Player>();
        for (int i = 1; i <= numPlayers; i++)

        {
            Console.Write($"Enter the name for Player {i}: ");
            string playerName = Console.ReadLine();
            players.Add(new Player(playerName));
        }




        // Create a deck and shuffle it
        Deck deck = new Deck();
        deck.Shuffle();


        // Deal cards to players
        foreach (Player player in players)
        {
            for (int i = 0; i < 5; i++) // Deal 5 cards to each player
            {
                Card card = deck.DrawCard();
                player.AddCard(card);
            }
        }
        Console.WriteLine();
        WritePlayerHandsToTxtFile(players, "player_hands.txt");
        Console.WriteLine();

        Thread.Sleep(3000); //3k milliseconds = 3 seconds

        // Display each player's hand
        foreach (Player player in players)
        {
            DisplayPlayerHand(player);
        }



        foreach (Player player in players)
        {
            Console.WriteLine($"{player.Name}, pick a card to play: ");
            Card playedCard = player.PlayCard();
            Console.WriteLine($"{player.Name} played {playedCard.Color} {playedCard.Name}\n");
        }

        
        // this will clear out the player_hands.txt file after each game
        ClearPlayerHandsToTxtFile("player_hands.txt");

        static void ClearPlayerHandsToTxtFile(string fileName)
        {
            File.WriteAllText(fileName, string.Empty);
            Console.WriteLine($"{fileName} has been cleared.");
        }

        Console.ReadLine();
    }


    
    // This class will write what each player drew as a card during the game to a file
    static void WritePlayerHandsToTxtFile(List<Player> players, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (Player player in players)
            {
                writer.WriteLine($"{player.Name}'s hand:");
                foreach (Card card in player.Hand)
                {
                    writer.WriteLine($"{card.Color} {card.Name}");
                }
                writer.WriteLine();
            }
            Console.WriteLine($"Player hands written to {fileName}");

        }

    }
//This will display each players hand with the color and the name of each card
    static void DisplayPlayerHand(Player player){
        for (int i = 0; i < player.Hand.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {player.Hand[i].Color} {player.Hand[i].Name}");
        }
        Console.WriteLine();
    }

}


// a base class for any playable card
// demonstrates inheritance
abstract class PlayableCard
{
    public string Name { get; }
    protected PlayableCard(string name)
    {
        Name = name;
    }
}

// Card class representing each card in the deck
class Card : PlayableCard
{
    public string Color { get; }

    public Card(string name, string color) : base(name)
    {
        Color = color;
    }

    // Override ToString to provide a meaningful string representation
    public override string ToString()
    {
        return Name;
    }
}










// Deck class representing a deck of cards
class Deck
{
    private List<Card> cards = new List<Card>();

    public Deck()
    {
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        // Add 5 mythical creatures with their respective colors indicating the creatures (green)
        for (int i = 0; i < 3; i++)
        {
            cards.Add(new Card("Mythical Unicorn", "Green"));
            cards.Add(new Card("Trash Monster", "Green"));
            cards.Add(new Card("Fire Dragon", "Green"));
            cards.Add(new Card("Phoenix", "Green"));
            cards.Add(new Card("Flying Monkey", "Green"));

        }

        // Add 5x of resource cards
        for (int i = 0; i < 5; i++)
        {
            cards.Add(new Card("Raindrop", "Blue"));
            cards.Add(new Card("Downpour", "Blue"));
            cards.Add(new Card("Magical Mushroom", "Red"));
            cards.Add(new Card("Mystical Mallow", "Red"));
            cards.Add(new Card("Magical Carrot", "Red"));
           
        }
    }

    public void Shuffle()
    {
        Random random = new Random();
        cards = cards.OrderBy(card => random.Next()).ToList();
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("The deck is empty.");
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }
}

// Player class representing a player in the game
class Player : PlayableCard
{
    public List<Card> Hand { get; } = new List<Card>();

    public Player(string name) : base(name)
    {
    }

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    public Card PlayCard()
    {
        DisplayPlayerHand();

        Console.Write("Enter the number of the card you want to play: ");
        int selectedCardIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedCardIndex < 0 || selectedCardIndex >= Hand.Count)
        {
            Console.WriteLine("Invalid selection. Please choose a valid card.");
            return PlayCard();
        }

        Card selectedCard = Hand[selectedCardIndex];
        Hand.RemoveAt(selectedCardIndex);
        return selectedCard;
    }

    private void DisplayPlayerHand(){
        Console.WriteLine($"{Name}'s hand: ");

        for (int i = 0; i < Hand.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Hand[i].Color} {Hand[i].Name}");
        }
        Console.WriteLine();
    }

}
