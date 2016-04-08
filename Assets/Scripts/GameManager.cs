using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
    public int TotalPlayers = 4;
    public List<Player> Players = new List<Player>();
    public List<Player> RemotePlayers = new List<Player>();
    public bool isHost = false;
    public bool PublicLobby = false;
    public bool CurrentlyConnecting = false;

    public List<Mecha> frameList = new List<Mecha>();
    public List<Weapon> weaponList = new List<Weapon>();

    protected override void Init() {
        base.Init();

        Debug.Log("[GameManager] Init");
        this.Persist = true;
    }

    void Start () {
        Debug.Log("[GameManager] Start");
    }
	
	void Update () {
	
	}

    public int PlayerCount {
        get {
            return Players.Count;
        }
    }

    public void Log(string message) {
        GameObject TextTemp = GameObject.FindGameObjectWithTag("DebugLog");
        if (TextTemp != null) {
            Text LogText = TextTemp.GetComponent<Text>();
            LogText.text += "\n" + message;
        }
    }
}
