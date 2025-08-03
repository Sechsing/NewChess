using UnityEngine;
using ChessSharp;
using ChessSharp.SquareData;
using ChessSharp.Pieces;

public class InputHandler : MonoBehaviour
{
    public BoardManager boardManager;
    public PromotionPanel promotionPanel; // Reference to your promotion UI

    private Vector2Int? selectedSquare = null;
    private Square pendingSource;
    private Square pendingTarget;
    private Player pendingPlayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                BoardCell cell = hit.collider.GetComponent<BoardCell>();
                if (cell != null)
                {
                    HandleClick(cell.col, cell.row);
                }
            }
        }
    }

    private void HandleClick(int col, int row)
    {
        if (boardManager == null || boardManager.game == null)
        {
            Debug.LogError("BoardManager or ChessGame not assigned.");
            return;
        }

        if (selectedSquare == null)
        {
            Piece? piece = boardManager.game.Board[row][col];
            if (piece != null && piece.Owner == boardManager.game.WhoseTurn)
            {
                selectedSquare = new Vector2Int(col, row);
                Debug.Log($"Selected piece at ({col},{row})");
            }
        }
        else
        {
            Vector2Int from = selectedSquare.Value;
            Vector2Int to = new Vector2Int(col, row);

            Square source = new Square((File)from.x, (Rank)from.y);
            Square target = new Square((File)to.x, (Rank)to.y);
            Player currentPlayer = boardManager.game.WhoseTurn;
            Piece? piece = boardManager.game[source.File, source.Rank];

            selectedSquare = null;

            if (piece is Bombard)
            {
                Fire fire = new Fire(source, target, currentPlayer);
                if (boardManager.game.MakeFire(fire))
                {
                    Debug.Log("Bombard fired!");
                    boardManager.UpdateBoardVisuals();
                    return;
                }
            }

            // Check for pawn promotion
            if (piece is Pawn)
            {
                bool isPromotion =
                    (currentPlayer == Player.White && target.Rank == Rank.Ninth) ||
                    (currentPlayer == Player.Black && target.Rank == Rank.Second);

                if (isPromotion)
                {
                    // Store move data until user picks promotion piece
                    pendingSource = source;
                    pendingTarget = target;
                    pendingPlayer = currentPlayer;

                    Debug.Log("Promotion panel shown.");
                    promotionPanel.Show(currentPlayer, OnPromotionSelected);
                    return;
                }
            }

            // Regular move (non-promotion)
            Move move = new Move(source, target, currentPlayer);
            if (boardManager.game.MakeMove(move, false))
            {
                boardManager.UpdateBoardVisuals();
                Debug.Log("Piece moved.");
                if (boardManager.game.GameState != GameState.NotCompleted)
                    Debug.Log($"Game Over: {boardManager.game.GameState}");
            }
            else
            {
                Debug.Log("Invalid move.");
            }
        }
    }

    // Callback from promotion panel
    private void OnPromotionSelected(PawnPromotion chosenPiece)
    {
        Move move = new Move(pendingSource, pendingTarget, pendingPlayer, chosenPiece);


        if (boardManager.game.MakeMove(move, false))
        {
            boardManager.UpdateBoardVisuals();
            Debug.Log("Pawn promoted and moved.");

            if (boardManager.game.GameState != GameState.NotCompleted)
                Debug.Log($"Game Over: {boardManager.game.GameState}");
        }
        else
        {
            Debug.Log("Invalid promotion move.");
        }
    }
}
