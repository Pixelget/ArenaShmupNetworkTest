using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAlphaPulse : MonoBehaviour {

    public float speed = 1f;

    float alpha = 1f;
    bool decrease = true;

    Color tempColor;
    Text thisText;

	void Start () {
        thisText = GetComponent<Text>();
        tempColor = thisText.color;
    }
	
	void Update () {
        if (decrease) {
            alpha -= speed * Time.deltaTime;

            if (alpha < 0f) {
                alpha = 0f;
                decrease = false;
            }
        } else {
            alpha += speed * Time.deltaTime;

            if (alpha > 1f) {
                alpha = 1f;
                decrease = true;
            }
        }

        tempColor.a = alpha;
        thisText.color = tempColor;
    }
}
