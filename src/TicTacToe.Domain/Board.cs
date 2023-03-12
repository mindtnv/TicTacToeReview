namespace TicTacToe.Domain;

public class Board : IEquatable<Board>
{
    private readonly char[,] _board;
    public readonly int Size;

    public Board(int size) : this(size, new char[size, size])
    {
    }

    public Board(int size, char[,] board)
    {
        Size = size;
        _board = board;
    }

    public bool Equals(Board? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        if (other.Size != Size)
            return false;

        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (other.GetCell(i, j) != _board[i, j])
                    return false;

        return true;
    }

    public char GetCell(int row, int col) => _board[row, col];
    public bool IsEmptyCell(int row, int col) => _board[row, col] == default(char);

    public void PlaceSymbol(int row, int column, char symbol)
    {
        _board[row, column] = symbol;
    }

    public bool IsFull()
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (IsEmptyCell(i, j))
                    return false;

        return true;
    }

    public bool IsSymbolWon(char symbol)
    {
        // Check diagonals first
        if (IsSymbolWonWithMainDiagonal(symbol) || IsSymbolWonWithSideDiagonal(symbol))
            return true;

        // Check all columns and rows
        for (var i = 0; i < Size; i++)
            if (IsSymbolWonWithColumn(symbol, i) || IsSymbolWonWithRow(symbol, i))
                return true;

        return false;
    }

    private bool IsSymbolWonWithColumn(char symbol, int column)
    {
        for (var i = 0; i < Size; i++)
            if (_board[i, column] != symbol)
                return false;

        return true;
    }

    private bool IsSymbolWonWithRow(char symbol, int row)
    {
        for (var i = 0; i < Size; i++)
            if (_board[row, i] != symbol)
                return false;

        return true;
    }

    private bool IsSymbolWonWithMainDiagonal(char symbol)
    {
        for (var i = 0; i < Size; i++)
            if (_board[i, i] != symbol)
                return false;

        return true;
    }

    private bool IsSymbolWonWithSideDiagonal(char symbol)
    {
        for (var i = 0; i < Size; i++)
            if (_board[i, Size - i - 1] != symbol)
                return false;

        return true;
    }

    public static Board CreateFrom(IEnumerable<IEnumerable<char>> board)
    {
        var rows = board.ToArray();
        var result = new char[rows.Length, rows.Length];
        for (var i = 0; i < rows.Length; i++)
        {
            var row = rows[i].ToArray();
            for (var j = 0; j < rows.Length; j++)
                result[i, j] = row[j];
        }

        return new Board(rows.Length, result);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((Board) obj);
    }

    public override int GetHashCode() => HashCode.Combine(_board, Size);
}