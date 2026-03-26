using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameReviewPanel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Transform movesContent;
    public GameObject moveEntryPrefab;
    public Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void ShowGame(GameRecord record)
    {
        if (titleText != null)
            titleText.text = $"{record.date} - {record.result}";

        // Clear previous entries
        foreach (Transform child in movesContent)
            Destroy(child.gameObject);

        // Populate moves
        for (int i = 0; i < record.actions.Count; i++)
        {
            var action = record.actions[i];
            GameObject entry = Instantiate(moveEntryPrefab, movesContent);

            var tmp = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                string player = action.player == "White" ? "W" : "B";
                tmp.text = $"{i + 1}. {player}: {action.notation}";
            }
        }
    }
}
