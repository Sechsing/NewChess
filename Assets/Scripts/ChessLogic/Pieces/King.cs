using ChessSharp.SquareData;

namespace ChessSharp.Pieces;

/// <summary>Represents a king <see cref="Piece"/>.</summary>
public class King : Piece
{
    internal King(Player player) : base(player) { }
    
    internal override bool IsValidGameMove(Move move, ChessGame board)
    {
        // No need to do null checks here, this method isn't public and isn't annotated with nullable.
        // If the caller try to pass a possible null reference, the compiler should issue a warning.
        // TODO: Should I add [NotNull] attribute to the arguments? What's the benefit?
        // The arguments are already non-nullable.

        int absDeltaX = move.GetAbsDeltaX();
        int absDeltaY = move.GetAbsDeltaY();

        // Regular king move.
        if (move.GetAbsDeltaX() <= 1 && move.GetAbsDeltaY() <= 1)
        {
            return true;
        }
        // Not castle move.
        if (absDeltaX != 2 || absDeltaY != 0 || move.Source.File != File.E ||
            (move.Player == Player.White && move.Source.Rank != Rank.Second) ||
            (move.Player == Player.Black && move.Source.Rank != Rank.Ninth) ||
            (board.GameState == GameState.BlackInCheck || board.GameState == GameState.WhiteInCheck))
        {
            return false;
        }

        // White king-side castle move.
        if (move.Player == Player.White && move.Destination.File == File.G && board.CanWhiteCastleKingSide &&
            !board.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.Second)) &&
            new Rook(Player.White).Equals(board[File.H, Rank.Second]))
        {
            return !board.PlayerWillBeInCheck(
                new Move(move.Source, new Square(File.F, Rank.Second), move.Player));
        }

        // Black king-side castle move.
        if (move.Player == Player.Black && move.Destination.File == File.G && board.CanBlackCastleKingSide &&
            !board.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.Ninth)) &&
            new Rook(Player.Black).Equals(board[File.H, Rank.Ninth]))
        {
            return !board.PlayerWillBeInCheck(
                new Move(move.Source, new Square(File.F, Rank.Ninth), move.Player));
        }

        // White queen-side castle move.
        if (move.Player == Player.White && move.Destination.File == File.C && board.CanWhiteCastleQueenSide &&
            !board.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.Second)) &&
            new Rook(Player.White).Equals(board[File.A, Rank.Second]))
        {
            return !board.PlayerWillBeInCheck(
                new Move(move.Source, new Square(File.D, Rank.Second), move.Player));
        }

        // Black queen-side castle move.
        if (move.Player == Player.Black && move.Destination.File == File.C && board.CanBlackCastleQueenSide &&
            !board.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.Ninth)) &&
            new Rook(Player.Black).Equals(board[File.A, Rank.Ninth]))
        {
            return !board.PlayerWillBeInCheck(
                new Move(move.Source, new Square(File.D, Rank.Ninth), move.Player));
        }

        return false;

    }
}
