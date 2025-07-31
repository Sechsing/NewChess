using UnityEngine;

public class PieceView : MonoBehaviour {
    public ChessSharp.Pieces.Piece piece;

    public void Setup(ChessSharp.Pieces.Piece p) {
        piece = p;
    }
}
