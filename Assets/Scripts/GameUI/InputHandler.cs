using UnityEngine;
using ChessSharp;
using ChessSharp.SquareData;
using ChessSharp.Pieces;

public class InputHandler : MonoBehaviour
{
    public BoardManager boardManager;
    private Vector2Int? selectedSquare = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int col = Mathf.FloorToInt(worldPos.x);
            int row = Mathf.FloorToInt(worldPos.y);

            if (!IsInsideBoard(col, row)) return;

            HandleClick(col, row);
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
            // Selecting a piece
            Piece? piece = boardManager.game.Board[row][col];
            if (piece != null && piece.Owner == boardManager.game.WhoseTurn)
            {
                selectedSquare = new Vector2Int(col, row);
                Debug.Log($"Selected piece at ({col},{row})");
            }
        }
        else
        {
            // Attempt to move or fire
            Vector2Int from = selectedSquare.Value;
            Vector2Int to = new Vector2Int(col, row);

            Square source = new Square((File)from.x, (Rank)from.y);
            Square target = new Square((File)to.x, (Rank)to.y);
            Player currentPlayer = boardManager.game.WhoseTurn;
            Piece? piece = boardManager.game[source.File, source.Rank];

            bool moveSuccess = false;

            if (piece is Bombard)
            {
                Fire fire = new Fire(source, target, currentPlayer);
                moveSuccess = boardManager.game.MakeFire(fire);
                if (moveSuccess)
                    Debug.Log("Bombard fired!");
            }

            if (!moveSuccess)
            {
                Move move = new Move(source, target, currentPlayer);
                moveSuccess = boardManager.game.MakeMove(move, false);
                if (moveSuccess)
                    Debug.Log("Piece moved.");
            }

            if (moveSuccess)
            {
                boardManager.UpdateBoardVisuals();
                selectedSquare = null;

                if (boardManager.game.GameState != GameState.NotCompleted)
                {
                    Debug.Log($"Game Over: {boardManager.game.GameState}");
                }
            }
            else
            {
                Debug.Log("Invalid move.");
                selectedSquare = null;
            }
        }
    }

    private bool IsInsideBoard(int col, int row)
    {
        return col >= 0 && col < 8 && row >= 0 && row < 10;
    }
}
