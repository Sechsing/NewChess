using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;
    public float squareSize = 1f;

    public GameObject whitePawnPrefab;
    public GameObject blackPawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject blackRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject blackKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject blackBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;
    public GameObject whiteBombardPrefab;
    public GameObject blackBombardPrefab;

    private Dictionary<Type, GameObject> whitePiecePrefabs;
    private Dictionary<Type, GameObject> blackPiecePrefabs;

    void Start()
    {
        InitializePrefabDictionaries();
        GenerateBoard();
        PlacePiecesFromLogic(new ChessGame());
    }

    void InitializePrefabDictionaries()
    {
        whitePiecePrefabs = new Dictionary<Type, GameObject>
        {
            { typeof(Pawn), whitePawnPrefab },
            { typeof(Rook), whiteRookPrefab },
            { typeof(Knight), whiteKnightPrefab },
            { typeof(Bishop), whiteBishopPrefab },
            { typeof(Queen), whiteQueenPrefab },
            { typeof(King), whiteKingPrefab },
            { typeof(Bombard), whiteBombardPrefab }
        };

        blackPiecePrefabs = new Dictionary<Type, GameObject>
        {
            { typeof(Pawn), blackPawnPrefab },
            { typeof(Rook), blackRookPrefab },
            { typeof(Knight), blackKnightPrefab },
            { typeof(Bishop), blackBishopPrefab },
            { typeof(Queen), blackQueenPrefab },
            { typeof(King), blackKingPrefab },
            { typeof(Bombard), blackBombardPrefab }
        };
    }

    void GenerateBoard()
    {
        SpriteRenderer sr = lightSquarePrefab.GetComponent<SpriteRenderer>();
        float squareSize = sr.bounds.size.x;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                GameObject prefab = (row + col) % 2 == 0 ? lightSquarePrefab : darkSquarePrefab;
                Vector2 position = new Vector2(col * squareSize, row * squareSize);
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }

    void PlacePiecesFromLogic(ChessGame game)
    {
        float size = lightSquarePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int row = 0; row < game.Board.Length; row++)
        {
            for (int col = 0; col < game.Board[row].Length; col++)
            {
                var piece = game.Board[row][col];
                if (piece == null) continue;

                GameObject prefab = piece.Owner == Player.White
                    ? whitePiecePrefabs[piece.GetType()]
                    : blackPiecePrefabs[piece.GetType()];

                Vector3 position = new Vector3(col * size, row * size, -1); // Slight z offset to appear above squares
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }
}