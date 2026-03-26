using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameStorage
{
    private static string GamesDirectory => Path.Combine(Application.persistentDataPath, "games");

    public static void SaveGame(GameRecord record)
    {
        if (!Directory.Exists(GamesDirectory))
            Directory.CreateDirectory(GamesDirectory);

        string json = JsonUtility.ToJson(record, true);
        string path = Path.Combine(GamesDirectory, $"{record.id}.json");
        File.WriteAllText(path, json);
        Debug.Log($"Game saved to: {path}");
    }

    public static List<GameRecord> LoadAllGames()
    {
        var records = new List<GameRecord>();

        if (!Directory.Exists(GamesDirectory))
            return records;

        foreach (string file in Directory.GetFiles(GamesDirectory, "*.json"))
        {
            string json = File.ReadAllText(file);
            var record = JsonUtility.FromJson<GameRecord>(json);
            if (record != null)
                records.Add(record);
        }

        return records;
    }

    public static GameRecord LoadGame(string id)
    {
        string path = Path.Combine(GamesDirectory, $"{id}.json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameRecord>(json);
    }
}
