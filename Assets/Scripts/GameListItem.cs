using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameListItem : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI resultText;
    public Button selectButton;

    private GameRecord record;
    private Action<GameRecord> onSelected;

    public void Setup(GameRecord record, Action<GameRecord> onSelected)
    {
        this.record = record;
        this.onSelected = onSelected;

        if (dateText != null) dateText.text = record.date;
        if (resultText != null) resultText.text = record.result;

        if (selectButton != null)
            selectButton.onClick.AddListener(() => onSelected?.Invoke(record));
    }
}
