namespace TicTacToe.Domain;

public interface IGameRepository
{
    Task<Game?> GetGameAsync(string id);
    Task SaveGameAsync(string id, Game game);
}