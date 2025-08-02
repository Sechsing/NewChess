using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;

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

    public ChessGame game { get; private set; }

    private Dictionary<Type, GameObject> whitePiecePrefabs;
    private Dictionary<Type, GameObject> blackPiecePrefabs;

    private GameObject?[,] pieceObjects;
    private float squareSize;
    private int numRows;
    private int numCols;

    public float SquareSize => squareSize;

    void Start()
    {
        squareSize = lightSquarePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        InitializePrefabDictionaries();
        
        game = new ChessGame();
        numRows = game.Board.Length;
        numCols = game.Board[0].Length;
        pieceObjects = new GameObject?[numRows, numCols];

        GenerateBoard();
        DrawInitialBoard();
        CenterCamera();
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
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                GameObject prefab = (row + col) % 2 == 0 ? lightSquarePrefab : darkSquarePrefab;
                Vector2 position = new Vector2(col * squareSize, row * squareSize);
                GameObject square = Instantiate(prefab, position, Quaternion.identity, transform);
                square.AddComponent<BoxCollider2D>(); 
                BoardCell cell = square.AddComponent<BoardCell>();
                cell.row = row;
                cell.col = col;
            }
        }
    }

    void DrawInitialBoard()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Piece? piece = game.Board[row][col];
                if (piece == null) continue;

                GameObject prefab = GetPrefabForPiece(piece);
                Quaternion rotation = piece is Bombard
                    ? Quaternion.Euler(0, 0, piece.Owner == Player.White ? 90 : -90)
                    : Quaternion.identity;

                GameObject instance = Instantiate(prefab, new Vector3(col * squareSize, row * squareSize, -1), rotation, transform);
                pieceObjects[row, col] = instance;
            }
        }
    }

    public void UpdateBoardVisuals()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Piece? newPiece = game.Board[row][col];
                GameObject? existingGO = pieceObjects[row, col];

                if (newPiece == null)
                {
                    if (existingGO != null)
                    {
                        Destroy(existingGO);
                        pieceObjects[row, col] = null;
                    }
                }
                else
                {
                    if (existingGO != null)
                    {
                        if (!IsGameObjectCorrect(existingGO, newPiece))
                        {
                            Destroy(existingGO);
                            GameObject newGO = InstantiatePiece(newPiece, col, row);
                            pieceObjects[row, col] = newGO;
                        }
                        else
                        {
                            existingGO.transform.position = new Vector3(col * squareSize, row * squareSize, -1);
                        }
                    }
                    else
                    {
                        GameObject newGO = InstantiatePiece(newPiece, col, row);
                        pieceObjects[row, col] = newGO;
                    }
                }
            }
        }
    }

    GameObject InstantiatePiece(Piece piece, int col, int row)
    {
        GameObject prefab = GetPrefabForPiece(piece);
        Quaternion rotation = piece is Bombard
            ? Quaternion.Euler(0, 0, piece.Owner == Player.White ? 90 : -90)
            : Quaternion.identity;

        GameObject instance = Instantiate(prefab, new Vector3(col * squareSize, row * squareSize, -1), rotation, transform);
        instance.AddComponent<BoxCollider2D>();
        BoardCell cell = instance.AddComponent<BoardCell>();
        cell.row = row;
        cell.col = col;
        return instance;
    }

    bool IsGameObjectCorrect(GameObject go, Piece piece)
    {
        string expectedName = GetPrefabForPiece(piece).name.Replace("(Clone)", "");
        return go.name.StartsWith(expectedName);
    }

    GameObject GetPrefabForPiece(Piece piece)
    {
        return piece.Owner == Player.White
            ? whitePiecePrefabs[piece.GetType()]
            : blackPiecePrefabs[piece.GetType()];
    }

    void CenterCamera()
    {
        float boardWidth = numCols * squareSize;
        float boardHeight = numRows * squareSize;

        Camera.main.transform.position = new Vector3(boardWidth / 2f, boardHeight / 2f, -10f);
        Camera.main.orthographicSize = boardHeight / 2f + 0.5f;
    }
}
