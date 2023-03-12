using System.Net;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.ViewModels;
using TicTacToe.Domain;

namespace TicTacToe.Application.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GameController> _logger;

    public GameController(ILogger<GameController> logger, IGameRepository gameRepository)
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(Game), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<ActionResult<Game>> GetGameAsync([FromRoute] string id)
    {
        var game = await _gameRepository.GetGameAsync(id);
        if (game == null)
            return NotFound();

        return Ok(game);
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(CreateGameViewModel), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateGameViewModel>> CreateGameAsync([FromBody] CreateGameRequestModel requestModel)
    {
        try
        {
            var gameId = Guid.NewGuid().ToString();
            var playerOne = new Player(requestModel.PlayerOneId, 'X');
            var playerTwo = new Player(requestModel.PlayerTwoId, 'O');
            var game = new Game(playerOne, playerTwo, 3);
            _logger.LogInformation(
                "Creating game with id {GameId} for players with ids {PlayerOneId} and {PlayerTwoId}",
                gameId, playerOne.Id, playerTwo.Id);
            await _gameRepository.SaveGameAsync(gameId, game);
            return new CreateGameViewModel
            {
                GameId = gameId,
                PlayerOne = playerOne,
                PlayerTwo = playerTwo,
            };
        }
        catch (TicTacToeDomainException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("{gameId}/move")]
    [ProducesResponseType(typeof(Game), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Game>> MoveAsync([FromRoute] string gameId, [FromBody] MoveRequestModel requestModel)
    {
        try
        {
            var game = await _gameRepository.GetGameAsync(gameId);
            if (game == null)
                return NotFound();

            var player = game.PlayerOne.Id == requestModel.PlayerId
                ? game.PlayerOne
                : game.PlayerTwo.Id == requestModel.PlayerId
                    ? game.PlayerTwo
                    : null;

            if (player == null)
                return BadRequest();

            _logger.LogInformation("[{GameId}] Player ({PlayerId}) place {PlayerSymbol} at ({Row}, {Column})", gameId,
                game.CurrentPlayer.Id, game.CurrentPlayer.Symbol, requestModel.Row, requestModel.Column);
            game.MakeMove(requestModel.Row, requestModel.Column, player);
            await _gameRepository.SaveGameAsync(gameId, game);
            return game;
        }
        catch (TicTacToeDomainException e)
        {
            return BadRequest(e.Message);
        }
    }
}