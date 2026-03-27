using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ChessSharp;

public class GameController : MonoBehaviour
{
    public BoardManager boardManager;
    public BoardInputHandler boardInputHandler;
    public MoveRecordPanel moveRecordPanel;
    public Button undoButton;
    public GameOverPanel gameOverPanel;

    private ChessGame snapshot;

    private void Start()
    {
        if (undoButton != null)
        {
            undoButton.onClick.AddListener(OnUndoClicked);
            undoButton.interactable = false;
        }
    }

    public void OnBeforeAction()
    {
        snapshot = boardManager.game.DeepClone();
    }

    public void OnAfterAction()
    {
        if (undoButton != null)
            undoButton.interactable = true;

        var actions = boardManager.game.Actions;
        if (actions.Count > 0)
        {
            var lastAction = actions[actions.Count - 1];
            string player = lastAction.Player == Player.White ? "W" : "B";
            string notation = lastAction.ToNotation();

            if (moveRecordPanel != null)
                moveRecordPanel.AddEntry($"{actions.Count}. {player}: {notation}");
        }

        if (boardManager.game.GameState != GameState.NotCompleted
            && boardManager.game.GameState != GameState.WhiteInCheck
            && boardManager.game.GameState != GameState.BlackInCheck)
        {
            GameRecord record = GameRecord.FromGame(boardManager.game);
            GameStorage.SaveGame(record);
            Debug.Log($"Game saved. State: {boardManager.game.GameState}");

            if (gameOverPanel != null)
                gameOverPanel.Show(boardManager.game.GameState);

            if (undoButton != null)
                undoButton.interactable = false;
        }
    }

    public void OnUndoClicked()
    {
        if (snapshot == null) return;

        boardManager.RestoreGame(snapshot);
        boardInputHandler.ResetSelection();

        if (moveRecordPanel != null)
            moveRecordPanel.RemoveLastEntry();

        snapshot = null;

        if (undoButton != null)
            undoButton.interactable = false;

        Debug.Log("Undo performed.");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
