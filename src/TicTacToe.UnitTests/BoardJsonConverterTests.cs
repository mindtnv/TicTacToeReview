using System.Text.Json;
using FluentAssertions;
using TicTacToe.Domain;
using TicTacToe.Infrastructure;

namespace TicTacToe.UnitTests;

[TestFixture]
public class BoardJsonConverterTests
{
    [Test]
    public void ReadTest()
    {
        var jsonString = """[["X","\u0000","\u0000"],["\u0000","0","\u0000"],["X","0","\u0000"]]""";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new BoardJsonConverter());
        var board = JsonSerializer.Deserialize<Board>(jsonString, options);
        board.Should().NotBeNull();
        board!.Size.Should().Be(3);
        board.GetCell(0, 0).Should().Be('X');
        board.GetCell(0, 1).Should().Be(default);
        board.GetCell(0, 2).Should().Be(default);
        board.GetCell(1, 0).Should().Be(default);
        board.GetCell(1, 1).Should().Be('0');
        board.GetCell(1, 2).Should().Be(default);
        board.GetCell(2, 0).Should().Be('X');
        board.GetCell(2, 1).Should().Be('0');
        board.GetCell(2, 2).Should().Be(default);
    }

    [Test]
    public void WriteReadTest()
    {
        var board = Board.CreateFrom(new[]
        {
            new[] {'X', default, default},
            new[] {default, '0', default},
            new[] {'X', '0', default},
        });
        var options = new JsonSerializerOptions();
        options.Converters.Add(new BoardJsonConverter());
        var json = JsonSerializer.Serialize(board, options);
        var parsedBoard = JsonSerializer.Deserialize<Board>(json, options);
        parsedBoard.Should().BeEquivalentTo(board);
    }

    [Test]
    public void WriteTest()
    {
        var jsonString = """[["X","\u0000","\u0000"],["\u0000","0","\u0000"],["X","0","\u0000"]]""";
        var board = Board.CreateFrom(new[]
        {
            new[] {'X', default, default},
            new[] {default, '0', default},
            new[] {'X', '0', default},
        });
        var options = new JsonSerializerOptions();
        options.Converters.Add(new BoardJsonConverter());
        var json = JsonSerializer.Serialize(board, options);
        json.Should().Be(jsonString);
    }
}