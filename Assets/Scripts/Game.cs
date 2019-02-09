using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public GameData ServerState;
    public GameData LocalState;

    private static Game instance;
    
    public static void AddTurnData (string key, string data)
    {
        instance.LocalState.RegisterGameState (key, data);
    }

    public static bool GetServerState<T> (string key, ref T data) where T : Object
    {
        return instance.ServerState.TryGetGameBehaviourData (key, ref data);
    }

    public static bool EndTurn ()
    {
        return false;
    }
}
