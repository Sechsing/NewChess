using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamesListPage : MonoBehaviour
{
    public Transform listContent;
    public GameObject gameListItemPrefab;
    public GameReviewPanel reviewPanel;

    private void Start()
    {
        LoadGamesList();
    }

    private void LoadGamesList()
    {
        List<GameRecord> games = GameStorage.LoadAllGames();

        foreach (var record in games)
        {
            GameObject item = Instantiate(gameListItemPrefab, listContent);
            var listItem = item.GetComponent<GameListItem>();
            if (listItem != null)
            {
                listItem.Setup(record, OnGameSelected);
            }
        }
    }

    private void OnGameSelected(GameRecord record)
    {
        if (reviewPanel != null)
        {
            reviewPanel.gameObject.SetActive(true);
            reviewPanel.ShowGame(record);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
