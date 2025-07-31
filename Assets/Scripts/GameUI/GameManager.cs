using UnityEngine;
using ChessSharp;

public class GameManager : MonoBehaviour {
    // public BoardManager boardManager;
    // public Game game;
    // private Square selected;

    // void Start() {
    //     game = new Game(); // or use custom 10x8 game setup
    //     boardManager.DrawPieces();
    // }

    // public ChessSharp.Pieces.Piece GetPieceAt(int file, int rank) {
    //     return game.Board[file, rank].Piece;
    // }

    // public void OnSquareClicked(int file, int rank) {
    //     if (selected == null) {
    //         if (game.Board[file, rank].Piece?.Player == game.WhoseTurn)
    //             selected = new Square(file, rank);
    //     } else {
    //         var move = new Move(selected, new Square(file, rank), game.WhoseTurn);
    //         if (game.IsMoveLegal(move)) {
    //             game.MakeMove(move);
    //             selected = null;
    //             boardManager.DrawPieces();
    //         } else {
    //             selected = null;
    //         }
    //     }
    // }
}
