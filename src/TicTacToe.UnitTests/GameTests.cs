using FluentAssertions;
using TicTacToe.Domain;

namespace TicTacToe.UnitTests;

[TestFixture]
[Category("Unit")]
public class GameTests
{
    [SetUp]
    public void SetUp()
    {
        PlayerOne = new Player(1, '+');
        PlayerTwo = new Player(2, '0');
        Game = new Game(PlayerOne, PlayerTwo, 3);
        Game.Winner.Should().BeNull();
        Game.IsOver.Should().BeFalse();
    }

    protected Player PlayerOne = null!;
    protected Player PlayerTwo = null!;
    protected Game Game = null!;

    [Test]
    public void IsDrawTest()
    {
        // + + 0
        // 0 0 +
        // + + 0
        Game.MakeMove(0, 0, PlayerOne);
        Game.MakeMove(1, 0, PlayerTwo);
        Game.MakeMove(0, 1, PlayerOne);
        Game.MakeMove(1, 1, PlayerTwo);
        Game.MakeMove(1, 2, PlayerOne);
        Game.MakeMove(0, 2, PlayerTwo);
        Game.MakeMove(2, 0, PlayerOne);
        Game.MakeMove(2, 2, PlayerTwo);
        Game.MakeMove(2, 1, PlayerOne);

        Game.IsOver.Should().BeTrue();
        Game.IsDraw.Should().BeTrue();
    }

    [Test]
    public void PlayerOneWonTest()
    {
        // + + .
        // 0 0 .
        // . . .
        for (var i = 0; i < Game.Size - 1; i++)
        {
            Game.MakeMove(0, i, PlayerOne);
            Game.MakeMove(1, i, PlayerTwo);
        }

        // + + +
        // 0 0 .
        // . . .
        Game.MakeMove(0, Game.Size - 1, PlayerOne);
        Game.IsOver.Should().BeTrue();
        Game.IsDraw.Should().BeFalse();
        Game.Winner.Should().NotBeNull().And.Subject.As<Player>().Id.Should().Be(PlayerOne.Id);
    }

    [Test]
    public void PlayerTwoWonTest()
    {
        // + + .
        // 0 0 .
        // . . .
        for (var i = 0; i < Game.Size - 1; i++)
        {
            Game.MakeMove(0, i, PlayerOne);
            Game.MakeMove(1, i, PlayerTwo);
        }

        // + + .
        // 0 0 .
        // . . +
        Game.MakeMove(Game.Size - 1, Game.Size - 1, PlayerOne);

        // + + .
        // 0 0 0
        // . . +
        Game.MakeMove(1, Game.Size - 1, PlayerTwo);
        Game.IsOver.Should().BeTrue();
        Game.IsDraw.Should().BeFalse();
        Game.Winner.Should().NotBeNull().And.Subject.As<Player>().Id.Should().Be(PlayerTwo.Id);
    }

    [Test]
    public void ThrowOnInvalidPlayerId()
    {
        // + . .
        // . . .
        // . . .
        Game.MakeMove(0, 0, PlayerOne);

        // + 0 .
        // . . .
        // . . .
        var move = () => Game.MakeMove(0, 1, new Player(-1, '0'));
        move.Should().Throw<TicTacToeDomainException>();
    }

    [Test]
    public void ThrowOnInvalidPlayerSymbol()
    {
        // + . .
        // . . .
        // . . .
        Game.MakeMove(0, 0, PlayerOne);

        // + - .
        // . . .
        // . . .
        var move = () => Game.MakeMove(0, 1, new Player(2, '-'));
        move.Should().Throw<TicTacToeDomainException>();
    }

    [Test]
    public void ThrowOnInvalidPlayerTurn()
    {
        // + . .
        // . . .
        // . . .
        Game.MakeMove(0, 0, PlayerOne);

        // + + .
        // . . .
        // . . .
        var move = () => Game.MakeMove(0, 1, PlayerOne);
        move.Should().Throw<TicTacToeDomainException>();
    }

    [Test]
    public void ThrowOnReplacingSymbol()
    {
        // + . .
        // . . .
        // . . .
        Game.MakeMove(0, 0, PlayerOne);

        // 0 . .
        // . . .
        // . . .
        var move = () => Game.MakeMove(0, 0, PlayerTwo);
        move.Should().Throw<TicTacToeDomainException>();
    }

    [Test]
    public void TrowOnGameOverTest()
    {
        // + + .
        // 0 0 .
        // . . .
        for (var i = 0; i < Game.Size - 1; i++)
        {
            Game.MakeMove(0, i, PlayerOne);
            Game.MakeMove(1, i, PlayerTwo);
        }

        // + + +
        // 0 0 .
        // . . .
        Game.MakeMove(0, Game.Size - 1, PlayerOne);
        Game.IsOver.Should().BeTrue();
        var move = () => Game.MakeMove(1, Game.Size - 1, PlayerOne);
        move.Should().Throw<TicTacToeDomainException>();
    }
}