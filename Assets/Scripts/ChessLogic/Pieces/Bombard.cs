namespace ChessSharp.Pieces;

/// <summary>Represents a bombard <see cref="Piece"/>.</summary>
public class Bombard : Piece
{
    internal Bombard(Player player) : base(player) { }

    internal override bool IsValidGameMove(Move move, ChessGame board)
    {
        return ((move.GetAbsDeltaX() == 1 && move.GetAbsDeltaY() == 0) ||
            (move.GetAbsDeltaY() == 1 && move.GetAbsDeltaX() == 0)) && 
                !board.IsTherePieceInBetween(move.Source, move.Destination);
    }

    internal bool IsValidFire(Fire fire, ChessGame board)
    {
        int forwardRange = fire.Player == Player.White ? 3 : -3;

        return ((fire.GetAbsDeltaX() == 0) && (fire.GetDeltaY() == forwardRange) ||
            (fire.GetAbsDeltaX() == 2) && (fire.GetDeltaY() == forwardRange)) &&
                !board.IsTherePieceInBetween(fire.Source, fire.Target);
    }
}