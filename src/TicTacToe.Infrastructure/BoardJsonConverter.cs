using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Domain;

namespace TicTacToe.Infrastructure;

public class BoardJsonConverter : JsonConverter<Board>
{
    public override Board? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new List<List<char>>();
        if (reader.TokenType != JsonTokenType.StartArray)
            return null;

        reader.Read();
        while (reader.TokenType == JsonTokenType.StartArray)
        {
            var row = new List<char>();
            reader.Read();
            while (reader.TokenType == JsonTokenType.String)
            {
                row.Add(char.Parse((reader.GetString() ?? default) ?? string.Empty));
                reader.Read();
            }

            if (reader.TokenType == JsonTokenType.EndArray)
            {
                result.Add(row);
                reader.Read();
            }
            else
            {
                return null;
            }
        }

        if (reader.TokenType != JsonTokenType.EndArray)
            return null;

        return Board.CreateFrom(result);
    }

    public override void Write(Utf8JsonWriter writer, Board value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        for (var i = 0; i < value.Size; i++)
        {
            writer.WriteStartArray();
            for (var j = 0; j < value.Size; j++)
                writer.WriteStringValue(value.GetCell(i, j).ToString());
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}