using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when Play Button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Called when Settings Button is clicked
    public void OpenSettings()
    {
        // For now, just log. Later, open a settings panel.
        Debug.Log("Settings menu opened!");
    }

    // Called when Quit Button is clicked
    public void QuitGame()
    {
        Debug.Log("Game quit!");
        Application.Quit();

        // Note: Application.Quit() won’t work in the editor.
        // It only works in a built game.
    }
}
