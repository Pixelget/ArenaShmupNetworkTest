using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameLobby {
    public int ID;
    public string HostIP;
    public ushort HostPort;
    public int PlayerCount;

    public GameLobby(int id, string ip, ushort port, int count) {
        ID = id;
        HostIP = ip;
        HostPort = port;
        PlayerCount = count;
    }
}
