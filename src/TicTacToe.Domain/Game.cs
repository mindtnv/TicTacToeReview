namespace TicTacToe.Domain;

public class Game
{
    // This simplifies the model in contrast to Player[]
    public Player PlayerOne { get; set; }
    public Player PlayerTwo { get; set; }
    public Player CurrentPlayer { get; set; }
    public Player? Winner { get; set; }
    public Board CurrentBoard { get; set; }
    public int Size { get; set; }
    public bool IsOver => CurrentBoard.IsFull() || Winner != null;
    public bool IsDraw => CurrentBoard.IsFull() && Winner == null;

    public Game()
    {
    }

    public Game(Player playerOne, Player playerTwo, int boardSize)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        CurrentPlayer = playerOne;
        CurrentBoard = new Board(boardSize);
        Size = boardSize;
    }

    public void MakeMove(int row, int column, Player player)
    {
        if (CurrentPlayer.Id != player.Id)
            throw new TicTacToeDomainException($"Is is turn player with id {CurrentPlayer.Id}");

        if (CurrentPlayer.Symbol != player.Symbol)
            throw new TicTacToeDomainException($"Invalid symbol for player id {CurrentPlayer.Id}");

        if (IsOver)
            throw new TicTacToeDomainException("Game already over");

        if (!CurrentBoard.IsEmptyCell(row, column))
            throw new TicTacToeDomainException($"Cell ({row}, {column}) not empty");

        CurrentBoard.PlaceSymbol(row, column, CurrentPlayer.Symbol);

        if (CurrentBoard.IsSymbolWon(CurrentPlayer.Symbol))
            Winner = CurrentPlayer;

        if (!IsOver)
            CurrentPlayer = CurrentPlayer.Id == PlayerOne.Id ? PlayerTwo : PlayerOne;
    }
}