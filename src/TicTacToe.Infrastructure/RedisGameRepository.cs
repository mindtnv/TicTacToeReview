using System.Text.Json;
using StackExchange.Redis;
using TicTacToe.Domain;

namespace TicTacToe.Infrastructure;

public class RedisGameRepository : IGameRepository
{
    private readonly IDatabase _database;

    public RedisGameRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<Game?> GetGameAsync(string id)
    {
        var redisValue = await _database.StringGetAsync(GetGameKey(id));
        if (redisValue.IsNull)
            return null;

        var bytes = (byte[]) redisValue!;
        using var stream = new MemoryStream(bytes);
        var options = new JsonSerializerOptions();
        options.Converters.Add(BoardJsonConverter.Shared);
        var basket = await JsonSerializer.DeserializeAsync<Game>(stream, options);
        return basket;
    }

    public async Task SaveGameAsync(string id, Game game)
    {
        using var stream = new MemoryStream();
        var options = new JsonSerializerOptions();
        options.Converters.Add(BoardJsonConverter.Shared);
        await JsonSerializer.SerializeAsync(stream, game, options);
        await _database.StringSetAsync(GetGameKey(id), RedisValue.CreateFrom(stream));
    }

    private string GetGameKey(string gameId) => $"game:{gameId}";
}