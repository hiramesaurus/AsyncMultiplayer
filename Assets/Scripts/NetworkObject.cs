using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public string NetworkId;

    public bool SyncTransform;
    private Transform attachedTransform;

    private void Awake ()
    {
        attachedTransform = GetComponent<Transform> ();
    }

    public void OnNetworkSerialize ()
    {
        if (SyncTransform)
            Game.AddTurnData (NetworkId, JsonUtility.ToJson (transform));
    }

    public void OnNetworkDeserialize ()
    {
        Game.GetServerState (NetworkId, ref attachedTransform);
    }
}
