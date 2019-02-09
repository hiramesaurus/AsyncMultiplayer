using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCreator : MonoBehaviour
{
    [Header ("Create Game")] public Button CreateGameButton;

    [Header ("Join Game")] public TMPro.TMP_InputField JoinGameName;
    public Button JoinButton;

    private bool isValidated;

    public void JoinGame ()
    {
        if (!isValidated)
            return;
        
        SetButtonInteractability (false);
        ServerConnector.ConnectToGame (JoinGameName.text, OnConnectedToGame);
    }

    public void CreateGame ()
    {
        SetButtonInteractability (false);
        ServerConnector.CreateGame (OnGameCreated);
    }

    private void OnConnectedToGame (bool result)
    {
        Debug.Log ($"Connected to game with result {result.ToString ()}");
        SetButtonInteractability (!result);
    }

    private void OnGameCreated (bool result)
    {
        Debug.Log ($"Game Created with result {result.ToString ()}");
        SetButtonInteractability (!result);
    }

    private void SetButtonInteractability (bool state)
    {
        if (JoinButton != null)
            JoinButton.interactable = state;
        if (CreateGameButton != null)
            CreateGameButton.interactable = state;
    }

    private void Start ()
    {
        if (JoinButton != null)
            JoinButton.onClick.AddListener (JoinGame);
        if (CreateGameButton != null)
            CreateGameButton.onClick.AddListener (CreateGame);  

        var lastGame = PlayerPrefs.GetString ("lastGameId", null);
        if (lastGame != null)
            JoinGameName.text = lastGame;
    }

    private void Update ()
    {
        isValidated = JoinGameName.text.Length > 10;
        JoinButton.interactable = isValidated;
    }
}