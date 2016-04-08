using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

using BeardedManStudios.Network;

public class JoinNetworkStarter : MonoBehaviour {

    public GameObject textInput;
    public GameObject HostIP;

    public Text IPDisplay;

	void Start () {
        // Variable space based on the amount of players in the current game
        int slots = GameManager.Instance.TotalPlayers - GameManager.Instance.PlayerCount - 1;
        if (slots > 0)
            GetComponent<BeardedManStudios.Forge.Examples.StartGame>().playerCount = slots;
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(GetComponent<BeardedManStudios.Forge.Examples.StartGame>().sceneName);

	    if (GameManager.Instance.isHost) {
            textInput.SetActive(false);
            string pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            IPDisplay.text = "Public IP: " + pubIp + "\nLocal IP: " + Networking.GetLocalIPAddress();
        }
	}
	
	void Update () {
        if (GameManager.Instance.isHost) {
            InputDevice device = InputManager.ActiveDevice;

            if (device.Action1.WasPressed) {
                Debug.Log("Starting Server");
                GetComponent<BeardedManStudios.Forge.Examples.StartGame>().StartServer();
            }
        }
    }
}
