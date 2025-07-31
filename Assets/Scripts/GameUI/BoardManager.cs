using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    // public GameObject squarePrefab;
    // public Transform boardParent;
    // public GameObject[] piecePrefabs; // Drag-and-drop in inspector
    // public GameManager gameManager;

    // private Dictionary<(int, int), GameObject> squareMap = new();

    // void Start() {
    //     CreateBoard();
    //     DrawPieces();
    // }

    // void CreateBoard() {
    //     for (int rank = 0; rank < 10; rank++) {
    //         for (int file = 0; file < 8; file++) {
    //             GameObject square = Instantiate(squarePrefab, boardParent);
    //             square.name = $"Square_{file}_{rank}";
    //             square.GetComponent<SquareView>().Setup(file, rank, gameManager);
    //             squareMap[(file, rank)] = square;
    //         }
    //     }
    // }

    // public void DrawPieces() {
    //     foreach (Transform child in boardParent) {
    //         var pv = child.GetComponentInChildren<PieceView>();
    //         if (pv != null) Destroy(pv.gameObject);
    //     }

    //     for (int rank = 0; rank < 10; rank++) {
    //         for (int file = 0; file < 8; file++) {
    //             var piece = gameManager.GetPieceAt(file, rank);
    //             if (piece != null) {
    //                 GameObject prefab = GetPrefabForPiece(piece);
    //                 GameObject pieceGO = Instantiate(prefab, squareMap[(file, rank)].transform);
    //                 pieceGO.GetComponent<PieceView>().Setup(piece);
    //             }
    //         }
    //     }
    // }

    // GameObject GetPrefabForPiece(ChessSharp.Pieces.Piece piece) {
    //     // You can use tags or names to map here
    //     string name = piece.GetType().Name + (piece.Player == ChessSharp.Player.White ? "White" : "Black");
    //     return System.Array.Find(piecePrefabs, p => p.name == name);
    // }
}
