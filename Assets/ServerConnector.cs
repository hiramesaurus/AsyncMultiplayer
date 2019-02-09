using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// http://dreamlo.com/lb/LRTrc3KZKEmUpAV0ZdjBlQ7AcRJwFRjkW3KCc4AH0D_A
public class ServerConnector : MonoBehaviour
{
    public enum ConnectionState
    {
        None,
        Connecting,
        Creating,
        Connected
    }

    private const string MainUrl = "http://dreamlo.com/lb/";
    private const string PrivateCode = "LRTrc3KZKEmUpAV0ZdjBlQ7AcRJwFRjkW3KCc4AH0D_A";
    private const string PublicCode = "5c5e1070b639870c248ed8a7";

    private static ServerConnector instance;

    [SerializeField] private string activeGameId;
    private ConnectionState currentConnectionState;

    public string ActiveGameId => activeGameId;

    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize ()
    {
        if (instance != null)
        {
            Destroy (instance.gameObject);
        }

        instance = new GameObject ("Server Connector").AddComponent<ServerConnector> ();
    }

    private static bool IsFreeForOperation ()
    {
        return instance.currentConnectionState == ConnectionState.None;
    }

    #region Create Game

    public static string CreateGame (Action<bool> callback)
    {
        Debug.Log ("Start game creation");
        if (!IsFreeForOperation ())
        {
            callback.Invoke (false);
            return string.Empty;
        }

        instance.activeGameId = Guid.NewGuid ().ToString ();

        PlayerPrefs.SetString ("lastGameId", instance.activeGameId);
        PlayerPrefs.Save ();

        instance.StartCoroutine (instance.Internal_CreateGame (callback));
        return instance.activeGameId;
    }

    private IEnumerator Internal_CreateGame (Action<bool> callback)
    {
        //
        var epoch = (DateTime.UtcNow - new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        Debug.Log (epoch.ToString ());

        using (var webRequest = new UnityWebRequest (
            $"{MainUrl}{PrivateCode}/add/{activeGameId}/{epoch.ToString ()}"))
        {
            yield return webRequest.SendWebRequest ();

            if (webRequest.isNetworkError)
            {
                Debug.LogError (
                    $"[{nameof (ServerConnector)}]: Failed to upload user's score! {webRequest.error}");
                callback?.Invoke (false);
                yield break;
            }

            callback?.Invoke (true);
        }
    }

    #endregion


    #region Connect to Game

    public static void ConnectToGame (string gameId, Action<bool> callback)
    {
        Debug.Log ("Start Connecting");
        if (!IsFreeForOperation ())
        {
            callback.Invoke (false);
            return;
        }

        instance.StartCoroutine (instance.Internal_ConnectToGame (gameId, callback));
    }

    private IEnumerator Internal_ConnectToGame (string gameId, Action<bool> callback)
    {
        var request = $"{MainUrl}{PublicCode}/pipe/{gameId}";
        Debug.Log (request);
        using (var webRequest = new UnityWebRequest (request))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer ();
            yield return webRequest.SendWebRequest ();

            if (webRequest.isNetworkError)
            {
                Debug.LogError ($"[{nameof (ServerConnector)}]: Failed to fetch user's score! {webRequest.error}");
                callback?.Invoke (false);
                yield break;
            }

            Debug.Log ($"GameData: {webRequest.downloadHandler.text}");
            callback?.Invoke (true);
        }
    }

    #endregion
}
