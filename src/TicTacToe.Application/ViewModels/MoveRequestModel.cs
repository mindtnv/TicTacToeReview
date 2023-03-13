namespace TicTacToe.Application.ViewModels;

public class MoveRequestModel
{
    public int PlayerId { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}