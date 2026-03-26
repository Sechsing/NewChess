using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveRecordPanel : MonoBehaviour
{
    public Transform content;
    public GameObject entryPrefab;

    private List<GameObject> entries = new List<GameObject>();

    public void AddEntry(string text)
    {
        if (entryPrefab == null || content == null) return;

        GameObject entry = Instantiate(entryPrefab, content);
        var tmp = entry.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = text;

        entries.Add(entry);
    }

    public void RemoveLastEntry()
    {
        if (entries.Count == 0) return;

        var last = entries[entries.Count - 1];
        entries.RemoveAt(entries.Count - 1);
        Destroy(last);
    }

    public void Clear()
    {
        foreach (var entry in entries)
            Destroy(entry);
        entries.Clear();
    }
}
