using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TicTacToe.Application.Controllers;
using TicTacToe.Application.ViewModels;
using TicTacToe.Domain;

namespace TicTacToe.UnitTests;

[TestFixture]
[Category("Unit")]
public class GameControllerTests
{
    [SetUp]
    public void SetUp()
    {
        PlayerOne = new Player(1, 'X');
        PlayerTwo = new Player(2, 'O');
        Game = new Game(PlayerOne, PlayerTwo, 3);
        GameId = "1";
        var logger = new Mock<ILogger<GameController>>().Object;
        var gameRepositoryMock = new Mock<IGameRepository>();

        gameRepositoryMock.Setup(x => x.GetGameAsync(It.Is(GameId, StringComparer.Ordinal)))
                          .ReturnsAsync((string _) => Game);

        var gameRepository = gameRepositoryMock.Object;

        Controller = new GameController(logger, gameRepository);
    }

    protected Player PlayerOne = null!;
    protected Player PlayerTwo = null!;
    protected Game Game = null!;
    protected string GameId = null!;
    protected GameController Controller = null!;

    [Test]
    public async Task CreateGameAsyncTest()
    {
        var requestModel = new CreateGameRequestModel
        {
            PlayerOneId = PlayerOne.Id,
            PlayerTwoId = PlayerTwo.Id,
        };
        var actionResult = await Controller.CreateGameAsync(requestModel);
        actionResult.Value.Should().NotBeNull();
        actionResult.Value!.GameId.Should().NotBeNull();
        actionResult.Value.PlayerOne.Should().BeEquivalentTo(PlayerOne);
        actionResult.Value.PlayerTwo.Should().BeEquivalentTo(PlayerTwo);
    }

    [Test]
    public async Task GetGameAsyncTest()
    {
        var foundedActionResult = await Controller.GetGameAsync(GameId);
        foundedActionResult.Result.Should().BeOfType<OkObjectResult>();
        foundedActionResult.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(Game);
    }

    [Test]
    public async Task MoveAsyncTest()
    {
        var requestModel = new MoveRequestModel
        {
            Column = 0,
            Row = 0,
            PlayerId = PlayerOne.Id,
        };
        var actionResult = await Controller.MoveAsync(GameId, requestModel);
        actionResult.Value.Should().NotBeNull();
        var game = actionResult.Value!;
        game.CurrentPlayer.Should().BeEquivalentTo(PlayerTwo);
    }
}