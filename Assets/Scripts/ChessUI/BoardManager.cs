using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using TMPro;
using TMPro.EditorUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;
    public GameObject promotionSquarePrefab;
    public GameObject dotPrefab;

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
    public GameObject haloPrefab;

    private GameObject haloInstance;

    private GameObject[,] cells;
    private GameObject?[,] pieceObjects;

    public ChessGame game { get; private set; }

    private List<GameObject> activeDots = new List<GameObject>();

    private Dictionary<Type, GameObject> whitePiecePrefabs;
    private Dictionary<Type, GameObject> blackPiecePrefabs;

    private Vector2 boardOffset;

    private string fileLabels = "ABCDEFGH";

    private float squareSize;
    private int numRows;
    private int numCols;

    public float SquareSize => squareSize;
    public int NumRows => numRows;
    public int NumCols => numCols;

    private void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();

        InitializePrefabDictionaries();

        game = new ChessGame();
        numRows = game.Board.Length;
        numCols = game.Board[0].Length;
        pieceObjects = new GameObject?[numRows, numCols];

        // Calculate square size dynamically 
        float width = rt.rect.width;
        float height = rt.rect.height;
        squareSize = Mathf.Min(width / numCols, height / numRows);

        // Offset for bottom-left spawn
        boardOffset = new Vector2(numCols * squareSize / 2f - squareSize / 2f, numRows * squareSize / 2f - squareSize / 2f);
    }

    private void Start()
    {
        GenerateBoard();
        GenerateBoardLabels();
        InstantiatePieces();
        UpdateBoardByGameState();
    }

    private void InitializePrefabDictionaries()
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

    private void GenerateBoard()
    {
        cells = new GameObject[numRows, numCols];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                GameObject prefab = (row + col) % 2 == 0 ? lightSquarePrefab : darkSquarePrefab;
                GameObject square = Instantiate(prefab, transform);

                RectTransform rt = square.GetComponent<RectTransform>();
                if (rt == null) rt = square.AddComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(col * squareSize, row * squareSize) - boardOffset;
                rt.sizeDelta = new Vector2(squareSize, squareSize);

                BoardCell cell = square.AddComponent<BoardCell>();
                cell.row = row;
                cell.col = col;
                cell.boardInputHandler = FindObjectOfType<BoardInputHandler>();

                cells[row, col] = square;
            }
        }
    }

    private void GenerateBoardLabels()
    {
        for (int col = 0; col < numCols; col++)
        {
            string letter = fileLabels[col].ToString();
            CreateLabel(letter, new Vector2(
                col * squareSize - boardOffset.x - squareSize/2f * 0.75f, 
                -boardOffset.y - squareSize/2f * 0.75f
            ));
        }

        for (int row = 0; row < numRows; row++)
        {
            string number = (row + 1).ToString();
            CreateLabel(number, new Vector2(
                -boardOffset.x - squareSize/2f * 0.75f, 
                row * squareSize - boardOffset.y + squareSize/2f * 0.75f
            ));
        }
    }

    private void CreateLabel(string text, Vector2 anchoredPos)
    {
        GameObject go = new GameObject("Label" + text);
        go.transform.SetParent(transform, false);

        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20; 
        tmp.alignment = TextAlignmentOptions.Center;

        Material mat = new Material(tmp.fontMaterial);
        mat.EnableKeyword("OUTLINE_ON");
        mat.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.1f);
        mat.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        tmp.fontMaterial = mat;
    }

    private void InstantiatePieces()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Piece? piece = game.Board[row][col];
                if (piece == null) continue;

                GameObject instance = InstantiatePiece(piece, col, row);
                pieceObjects[row, col] = instance;
            }
        }
    }

    private GameObject InstantiatePiece(Piece piece, int col, int row)
    {
        GameObject prefab = GetPrefabForPiece(piece);
        GameObject instance = Instantiate(prefab, transform);

        RectTransform rt = instance.GetComponent<RectTransform>();
        if (rt == null) rt = instance.AddComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(col * squareSize, row * squareSize) - boardOffset;

        // Default size
        float width = squareSize * 0.75f;
        float height = squareSize * 0.75f;

        // If piece is a pawn, adjust width
        if (piece is Pawn || piece is Rook)
        {
            width *= 0.85f; 
        }

        rt.sizeDelta = new Vector2(width, height);

        // Rotation handling for Bombard
        if (piece is Bombard)
            rt.localRotation = Quaternion.Euler(0, 0, piece.Owner == Player.White ? 90 : -90);
        else
            rt.localRotation = Quaternion.identity;

        return instance;
    }

    public GameObject InstantiateDot(int row, int col)
    {
        GameObject dot = Instantiate(dotPrefab, transform);

        RectTransform rt = dot.GetComponent<RectTransform>();
        if (rt == null) rt = dot.AddComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(col * squareSize, row * squareSize) - boardOffset;
        rt.sizeDelta = new Vector2(squareSize * 0.25f, squareSize * 0.25f); 

        activeDots.Add(dot);

        return dot;
    }

    public void ClearDots()
    {
        foreach (var dot in activeDots)
        {
            Destroy(dot);
        }
        activeDots.Clear();
    }

    public void TriggerPromotion(int row, int col, Player player, Action<PawnPromotion> callback)
    {
        bool isWhite = player == Player.White;
        int direction = isWhite ? -1 : 1;

        GameObject[] piecePrefabs = isWhite
            ? new[] { whiteQueenPrefab, whiteRookPrefab, whiteBishopPrefab, whiteKnightPrefab }
            : new[] { blackQueenPrefab, blackRookPrefab, blackBishopPrefab, blackKnightPrefab };

        PawnPromotion[] promotionTypes = {
            PawnPromotion.Queen,
            PawnPromotion.Rook,
            PawnPromotion.Bishop,
            PawnPromotion.Knight
        };

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            int optionRow = row + (i * direction);
            int optionCol = col;

            if (optionRow < 0 || optionRow >= numRows) continue;

            GameObject bg = Instantiate(promotionSquarePrefab, transform);
            bg.tag = "PromotionOption";

            RectTransform bgRT = bg.GetComponent<RectTransform>();
            if (bgRT == null) bgRT = bg.AddComponent<RectTransform>();

            bgRT.anchoredPosition = new Vector2(optionCol * squareSize, optionRow * squareSize) - boardOffset;
            bgRT.sizeDelta = new Vector2(squareSize, squareSize);

            PromotionOption option = bg.AddComponent<PromotionOption>();
            option.Init(promotionTypes[i], callback);

            GameObject piece = Instantiate(piecePrefabs[i], bg.transform);
            RectTransform pieceRT = piece.GetComponent<RectTransform>();
            if (pieceRT == null) pieceRT = piece.AddComponent<RectTransform>();

            pieceRT.anchoredPosition = Vector2.zero;
            pieceRT.sizeDelta = new Vector2(squareSize * 0.75f, squareSize * 0.75f);
        }
    }

    public void UpdateBoardByGameState()
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
                            RectTransform rt = existingGO.GetComponent<RectTransform>();
                            if (rt != null)
                            {
                                rt.anchoredPosition = new Vector2(col * squareSize, row * squareSize) - boardOffset;
                            }
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
        UpdateHalo(game.WhoseTurn);
    }

    private bool IsGameObjectCorrect(GameObject go, Piece piece)
    {
        string expectedName = GetPrefabForPiece(piece).name.Replace("(Clone)", "");
        return go.name.StartsWith(expectedName);
    }

    private GameObject GetPrefabForPiece(Piece piece)
    {
        return piece.Owner == Player.White
            ? whitePiecePrefabs[piece.GetType()]
            : blackPiecePrefabs[piece.GetType()];
    }

    public void UpdateHalo(Player currentPlayer)
    {
        // Destroy halo if game is over
        if (game.GameState != GameState.NotCompleted)
        {
            if (haloInstance != null)
            {
                Destroy(haloInstance);
                haloInstance = null;
            }
            return;
        }

        // Find the king of the current player
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Piece? piece = game.Board[row][col];
                if (piece is King && piece.Owner == currentPlayer)
                {
                    // Create halo if it doesn't exist
                    if (haloInstance == null)
                    {
                        haloInstance = Instantiate(haloPrefab, transform);
                    }

                    RectTransform rt = haloInstance.GetComponent<RectTransform>();
                    if (rt == null) rt = haloInstance.AddComponent<RectTransform>();

                    rt.anchoredPosition = new Vector2(col * squareSize, row * squareSize + 27.5f) - boardOffset;
                    rt.sizeDelta = new Vector2(squareSize * 0.75f, squareSize * 0.15f);

                    return;
                }
            }
        }
    }
}
