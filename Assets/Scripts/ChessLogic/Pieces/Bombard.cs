namespace ChessSharp.Pieces;

/// <summary>Represents a bombard <see cref="Piece"/>.</summary>
public class Bombard : Piece
{
    internal Bombard(Player player) : base(player) { }

    internal override bool IsValidGameMove(Move move, ChessGame board)
    {
        if (board[move.Destination.File, move.Destination.Rank] != null)
            return false;

        return ((move.GetAbsDeltaX() == 1 && move.GetAbsDeltaY() == 0) ||
            (move.GetAbsDeltaY() == 1 && move.GetAbsDeltaX() == 0)) && 
                !board.IsTherePieceInBetween(move.Source, move.Destination);
    }

    internal bool IsValidFire(Fire fire, ChessGame board)
    {
        int direction = fire.Player == Player.White ? 1 : -1;

        return ((fire.GetAbsDeltaX() == 0) && (fire.GetDeltaY() == 3 * direction) ||
            (fire.GetAbsDeltaX() == 2) && (fire.GetDeltaY() == 2 * direction)) &&
                !board.IsTherePieceInBetween(fire.Source, fire.Target);
    }
}