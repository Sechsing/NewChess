namespace ChessSharp.Pieces;

/// <summary>Represents a bombard <see cref="Piece"/>.</summary>
public class Bombard : Piece
{
    internal Bombard(Player player) : base(player) { }


    internal override bool IsValidGameMove(Move move, ChessGame board)
    {
        // No need to do null checks here, this method isn't public and isn't annotated with nullable.
        // If the caller try to pass a possible null reference, the compiler should issue a warning.
        // TODO: Should I add [NotNull] attribute to the arguments? What's the benefit?
        // The arguments are already non-nullable.
        return ((move.GetAbsDeltaX() == 1 && move.GetAbsDeltaY() == 0) ||
            (move.GetAbsDeltaY() == 1 && move.GetAbsDeltaX() == 0)) && 
                !board.IsTherePieceInBetween(move.Source, move.Destination);
    }
}