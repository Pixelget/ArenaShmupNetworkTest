using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class GameSetup : NetworkedMonoBehavior {
    
    public GameObject PlayerPrefab = null;

	void Start () {
        Log("Starting Game.");

        Log("There are " + GameManager.Instance.Players.Count.ToString() + " local players to spawn.");
        // Spawn Players based on player List
        for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
            Spawn(GameManager.Instance.Players[i]);
        }
	}
	
	void Update () {

	}

    void Spawn(Player player) {
        if (Networking.PrimarySocket.Connected) {
            Log("Spawning Player: " + player.Name);
            Networking.Instantiate("Player", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f), Quaternion.identity, NetworkReceivers.AllBuffered, (go) => PlayerCreated(go, player));
        } else {
            Networking.PrimarySocket.connected += delegate () {
                Networking.Instantiate("Player", new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f), Quaternion.identity, NetworkReceivers.AllBuffered, (go) => PlayerCreated(go, player));
            };

            Log("Spawning Player: " + player.Name + " - Delayed");
        }
    }

    void PlayerCreated(SimpleNetworkedMonoBehavior snmb, Player player) {
        //void PlayerCreated(GameObject go, Player player) {
        GameObject go = snmb.gameObject;
        go.GetComponent<PlayerManager>().InitPlayer(player);

    }

    void Log(string message) {
        GameManager.Instance.Log(message);
    }
}
