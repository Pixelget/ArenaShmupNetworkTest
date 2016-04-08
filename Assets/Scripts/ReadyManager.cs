using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReadyManager : MonoBehaviour {
    
    public Text ReadyText;

    int readyCount = 0;
    float timer = 3f;
    float TimeDelay = 3f;

    void Update () {
        readyCount = 0;

        for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
            if (GameManager.Instance.Players[i].Ready)
                readyCount++;
        }

        if (GameManager.Instance.Players.Count > 0) {
            if (readyCount == GameManager.Instance.Players.Count) {
                timer -= Time.deltaTime;

                if (timer < -1f) {
                    ReadyText.text = "Joined";
                    // Do Join stuff here

                    if (GameManager.Instance.PublicLobby)
                        SceneManager.LoadScene("JoinNetworkGame");
                    else
                        SceneManager.LoadScene("JoinNetworkGame"); // Not networked
                } else if (timer < 0f) {
                    ReadyText.text = "Joining Game";
                } else {
                    ReadyText.text = "Starting in " + Mathf.CeilToInt(timer) + " seconds.";
                }
            } else {
                timer = TimeDelay;
                ReadyText.text = "Waiting on " + (GameManager.Instance.Players.Count - readyCount) + " players.";
            }
        } else {
            ReadyText.text = "Waiting on players";
        }
    }
}
