using System;
using System.Collections.Generic;
using ChessSharp;
using UnityEngine;

[Serializable]
public class ActionRecord
{
    public string type;
    public string player;
    public string notation;
}

[Serializable]
public class GameRecord
{
    public string id;
    public string date;
    public string result;
    public List<ActionRecord> actions = new List<ActionRecord>();

    public static GameRecord FromGame(ChessGame game)
    {
        var record = new GameRecord
        {
            id = Guid.NewGuid().ToString(),
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            result = game.GameState.ToString()
        };

        foreach (var action in game.Actions)
        {
            record.actions.Add(new ActionRecord
            {
                type = action is Move ? "Move" : "Fire",
                player = action.Player.ToString(),
                notation = action.ToNotation()
            });
        }

        return record;
    }
}
