using FluentAssertions;
using StackExchange.Redis;
using TicTacToe.Domain;
using TicTacToe.Infrastructure;

namespace TicTacToe.FunctionalTests;

[TestFixture]
[Category("Functional")]
public class RedisGameRepositoryTests
{
    protected RedisGameRepository GameRepository = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        GameRepository = new RedisGameRepository(ConnectionMultiplexer.Connect("localhost"));
    }

    [Test]
    public async Task SaveGetGameTest()
    {
        var gameId = Guid.NewGuid().ToString();
        var playerOne = new Player(1, '+');
        var playerTwo = new Player(2, '0');
        var game = new Game(playerOne, playerTwo, 3);
        // + . .
        // . . .
        // . . 0
        game.MakeMove(0, 0, playerOne);
        game.MakeMove(game.Size - 1, game.Size - 1, playerTwo);
        await GameRepository.SaveGameAsync(gameId, game);
        var savedGame = await GameRepository.GetGameAsync(gameId);
        savedGame.Should().NotBeNull();
        savedGame.Should().BeEquivalentTo(game);
    }
}