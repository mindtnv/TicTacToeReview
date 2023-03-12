namespace TicTacToe.Domain;

public class Player
{
    public int Id { get; set; }
    public char Symbol { get; }

    public Player(int id, char symbol)
    {
        Id = id;
        Symbol = symbol;
    }
}