using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private Dictionary<string, string> gameDataMap = new Dictionary<string, string> ();

    public void RegisterGameState (string key, string data)
    {
        if (gameDataMap.ContainsKey (key))
        {
            gameDataMap[key] = data;
            return;
        }
        gameDataMap.Add (key, data);
    }
    
    public bool TryGetGameData<T> (string key, ref T data) where T : new ()
    {
        if (!gameDataMap.TryGetValue (key, out var json))
        {
            return false;
        }

        data = JsonUtility.FromJson<T> (json);
        return true;
    }

    public bool TryGetGameBehaviourData<T> (string key, ref T data) where T : Object
    {
        if (!gameDataMap.TryGetValue (key, out var json))
        {
            return false;
        }

        JsonUtility.FromJsonOverwrite (json, data);
        return true;
    }

}