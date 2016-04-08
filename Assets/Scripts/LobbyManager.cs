using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Network;

[System.Serializable]
public class LobbyManager : SimpleNetworkedMonoBehavior {
    #region Singleton Behaviour
    static LobbyManager _instance;
    static public bool isActive {
        get {
            return _instance != null;
        }
    }

    public static LobbyManager Instance{
        get {
            if (_instance == null) {
                _instance = Object.FindObjectOfType(typeof(LobbyManager)) as LobbyManager;

                if (_instance == null) {
                    GameObject go = new GameObject("LobbyManager");
                    _instance = go.AddComponent<LobbyManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    string matchlookupserverip = "127.0.0.1";
    ushort port = 19837;

    int ConnectionAttempts = 10;
    //float delayBetweenAttempts = 0.25f;

    //bool currentlyConnecting = false;
    int attempts = 0;

    bool isHosting = false;

    public List<GameLobby> GetHostedGames() {
        List<GameLobby> results = new List<GameLobby>();
        
        Networking.Connect(matchlookupserverip, port, Networking.TransportationProtocolType.UDP, true);
        //Networking.WriteTCP(Networking.Sockets[port], );

        return results;
    }

    public void CreateGame() {
        // Create a game and add the info to the server
        // Create the object
        // send it to the server
    }

    protected override void UnityUpdate() {
        base.UnityUpdate();

        if (isHosting) {
            // Call still active here every 30seconds
        }
    }

    public void EstablishNetworkGame() {
        Debug.Log("Connecting to network game");
        attempts = 0;
        //currentlyConnecting = true;

        TryToJoinGame();
    }
    public void DisconnectNetworkGame() {
        Debug.Log("Disconnecting from network game");
        Networking.Disconnect(port);
    }

    public void TryToJoinGame() {
        Debug.Log("Attempt " + attempts);
        List<GameLobby> NetworkGames = new List<GameLobby>();
        //while (attempts < ConnectionAttempts) {
        // Check for an active network game with enough space in the lobby (player count)
        // Get table/list of network game objects from server
        NetworkGames = GetHostedGames();

        // loop through each to see if it has enough space
        for (int i = 0; i < NetworkGames.Count; i++) {
            if (NetworkGames[i].PlayerCount + GameManager.Instance.Players.Count <= 4) {
                // if successful, attempt to connect
                Debug.Log("Connecting to " + NetworkGames[i].HostIP);
            }
        }

        attempts++;

        if (attempts > ConnectionAttempts) {
            Debug.Log("Connection Timeout - Creating Game");
            //if (!foundGame) {
            // Create a hosted lobby
        } else {
            TryToJoinGame();
        }
    }
}
