namespace TicTacToe.Domain;

public class TicTacToeDomainException : Exception
{
    public TicTacToeDomainException(string message) : base(message)
    {
    }
}