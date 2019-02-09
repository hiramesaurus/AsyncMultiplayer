using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private Dictionary<string, string> gameDataMap = new Dictionary<string, string> ();

    public bool TryGetGameData<T> (string key, out T data) where T : new ()
    {
        if (!gameDataMap.TryGetValue (key, out var json))
        {
            data = default (T);
            return false;
        }

        data = JsonUtility.FromJson<T> (json);
        return true;
    }

    public bool TryGetGameBehaviourData<T> (string key, T data) where T : Object
    {
        if (!gameDataMap.TryGetValue (key, out var json))
        {
            return false;
        }

        JsonUtility.FromJsonOverwrite (json, data);
        return true;
    }

}