using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugHostOrClientIndicator : MonoBehaviour {

    Text thisText;

	void Start () {
        thisText = GetComponent<Text>();
	}
	
	void Update () {
        if (GameManager.Instance.isHost)
            thisText.text = "Host";
        else
            thisText.text = "Client";
    }
}
