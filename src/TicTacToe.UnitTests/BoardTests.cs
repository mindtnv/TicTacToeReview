using FluentAssertions;
using TicTacToe.Domain;

namespace TicTacToe.UnitTests;

[TestFixture]
[Category("Unit")]
public class BoardTests
{
    [Test]
    public void IsEmptyCellTest()
    {
        var board = new Board(3);
        board.PlaceSymbol(0, 0, 'T');
        board.IsEmptyCell(0, 0).Should().BeFalse();
        board.IsEmptyCell(1, 1).Should().BeTrue();
    }

    [Test]
    public void IsFullTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size; i++)
            for (var j = 0; j < board.Size; j++)
                board.PlaceSymbol(i, j, 'T');

        board.IsFull().Should().BeTrue();
    }

    [Test]
    public void IsNotFullTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size - 1; i++)
            for (var j = 0; j < board.Size - 1; j++)
                board.PlaceSymbol(i, j, 'T');

        board.IsFull().Should().BeFalse();
    }

    [Test]
    public void SymbolWonWithColumnTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size - 1; i++)
            board.PlaceSymbol(i, 0, 'T');

        board.IsSymbolWon('T').Should().BeFalse();
        board.PlaceSymbol(board.Size - 1, 0, 'T');
        board.IsSymbolWon('T').Should().BeTrue();
    }

    [Test]
    public void SymbolWonWithMainDiagonalTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size - 1; i++)
            board.PlaceSymbol(i, i, 'T');

        board.IsSymbolWon('T').Should().BeFalse();
        board.PlaceSymbol(board.Size - 1, board.Size - 1, 'T');
        board.IsSymbolWon('T').Should().BeTrue();
    }

    [Test]
    public void SymbolWonWithRowTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size - 1; i++)
            board.PlaceSymbol(0, i, 'T');

        board.IsSymbolWon('T').Should().BeFalse();
        board.PlaceSymbol(0, board.Size - 1, 'T');
        board.IsSymbolWon('T').Should().BeTrue();
    }

    [Test]
    public void SymbolWonWithSideDiagonalTest()
    {
        var board = new Board(3);
        for (var i = 0; i < board.Size - 1; i++)
            board.PlaceSymbol(i, board.Size - i - 1, 'T');

        board.IsSymbolWon('T').Should().BeFalse();
        board.PlaceSymbol(board.Size - 1, 0, 'T');
        board.IsSymbolWon('T').Should().BeTrue();
    }
}