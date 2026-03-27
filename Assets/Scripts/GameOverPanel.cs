using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using ChessSharp;

public class GameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button historyButton;

    private void Awake()
    {
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        if (historyButton != null)
            historyButton.onClick.AddListener(() => SceneManager.LoadScene("GamesHistory"));
    }

    public void Show(GameState state)
    {
        if (resultText != null)
        {
            resultText.text = state switch
            {
                GameState.WhiteWinner => "White Wins!",
                GameState.BlackWinner => "Black Wins!",
                GameState.Stalemate => "Stalemate!",
                GameState.Draw => "Draw!",
                _ => "Game Over!"
            };
        }

        gameObject.SetActive(true);
    }
}
