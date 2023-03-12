using TicTacToe.Domain;

namespace TicTacToe.Application.ViewModels;

public class CreateGameViewModel
{
    public string GameId { get; set; }
    public Player PlayerOne { get; set; }
    public Player PlayerTwo { get; set; }
}