using UnityEngine;
using ChessSharp;
using ChessSharp.SquareData;
using ChessSharp.Pieces;

public class BoardInputHandler : MonoBehaviour
{
    public BoardManager boardManager;
    public GameController gameController;

    private Vector2Int? selectedSquare = null;
    private Square pendingSource;
    private Square pendingTarget;
    private Player pendingPlayer;

    public void OnCellClicked(int col, int row)
    {
        HandleClick(col, row);
    }
    
    private void HandleClick(int col, int row)
    {
        // If no piece is selected yet
        if (selectedSquare == null)
        {
            Piece? piece = boardManager.game.Board[row][col];
            if (piece != null && piece.Owner == boardManager.game.WhoseTurn)
            {
                selectedSquare = new Vector2Int(col, row);

                boardManager.ClearDots();
                Square source = new Square((File)col, (Rank)row);
                var newMoves = ChessUtilities.GetValidMovesOfSourceSquare(source, boardManager.game);

                foreach (var move in newMoves)
                {
                    int targetCol = (int)move.Destination.File;
                    int targetRow = (int)move.Destination.Rank;
                    boardManager.InstantiateDot(targetRow, targetCol);
                }
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

            // Switch selection if another piece of the same color is selected
            Piece? clickedPiece = boardManager.game.Board[row][col];
            if (clickedPiece != null && clickedPiece.Owner == currentPlayer)
            {
                selectedSquare = new Vector2Int(col, row);
                boardManager.ClearDots();

                Square newSource = new Square((File)col, (Rank)row);
                var newValidMoves = ChessUtilities.GetValidMovesOfSourceSquare(newSource, boardManager.game);

                foreach (var newMove in newValidMoves)
                {
                    int targetCol = (int)newMove.Destination.File;
                    int targetRow = (int)newMove.Destination.Rank;
                    boardManager.InstantiateDot(targetRow, targetCol);
                }

                Debug.Log($"Switched selection to piece at ({col},{row})");
                return;
            }

            // Otherwise clear dots and try a move
            boardManager.ClearDots();
            selectedSquare = null;

            if (piece is Bombard)
            {
                Fire fire = new Fire(source, target, currentPlayer);
                gameController?.OnBeforeAction();
                if (boardManager.game.MakeFire(fire))
                {
                    Debug.Log("Bombard fired!");
                    boardManager.UpdateBoardByGameState();
                    gameController?.OnAfterAction();
                    return;
                }
            }

            // Promotion logic
            if (piece is Pawn)
            {
                bool isPromotion =
                    (currentPlayer == Player.White && target.Rank == Rank.Ninth) ||
                    (currentPlayer == Player.Black && target.Rank == Rank.Second);

                if (isPromotion)
                {
                    int fileDiff = (int)target.File - (int)source.File;

                    if (fileDiff == 0)
                    {
                        if (boardManager.game.Board[(int)target.Rank][(int)target.File] == null)
                        {
                            pendingSource = source;
                            pendingTarget = target;
                            pendingPlayer = currentPlayer;

                            Debug.Log("Promotion panel shown.");
                            boardManager.TriggerPromotion(row, col, currentPlayer, OnPromotionSelected);
                            return;
                        }
                    }
                    else
                    {
                        pendingSource = source;
                        pendingTarget = target;
                        pendingPlayer = currentPlayer;

                        Debug.Log("Promotion panel shown (capture promotion).");
                        boardManager.TriggerPromotion(row, col, currentPlayer, OnPromotionSelected);
                        return;
                    }
                }
            }

            // Regular move
            if (piece == null) return;
            Move move = new Move(source, target, currentPlayer);
            gameController?.OnBeforeAction();
            if (boardManager.game.MakeMove(move, false))
            {
                boardManager.UpdateBoardByGameState();
                Debug.Log("Piece moved.");
                gameController?.OnAfterAction();
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

        gameController?.OnBeforeAction();
        if (boardManager.game.MakeMove(move, false))
        {
            boardManager.UpdateBoardByGameState();

            GameObject[] promos = GameObject.FindGameObjectsWithTag("PromotionOption");
            foreach (var promo in promos)
            {
                Destroy(promo);
            }

            gameController?.OnAfterAction();
            if (boardManager.game.GameState != GameState.NotCompleted)
                Debug.Log($"Game Over: {boardManager.game.GameState}");
        }
        else
        {
            Debug.Log("Invalid promotion move.");
        }
    }

    public void ResetSelection()
    {
        selectedSquare = null;
        boardManager.ClearDots();
    }
}
