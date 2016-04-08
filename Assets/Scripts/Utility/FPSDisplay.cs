using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour {
    float deltaTime = 0.0f;
    float[] avgFrames = new float[120];
    int frameIndex = 0;

    float avgTick = 1f;
    float timer = 0f;


    float avg = 0f;

    public bool ShowFPS = false;
    public bool ShowAverage = false;

    void Start() {
        for (int i = 0; i < avgFrames.Length; i++) {
            avgFrames[i] = 0f;
        }
    }

    void Update() {
        if (ShowFPS) {
            timer += Time.deltaTime;
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            avgFrames[frameIndex] = 1.0f / deltaTime;
            frameIndex++;
            if (frameIndex >= avgFrames.Length) {
                frameIndex = 0;
            }
        }
    }

    void OnGUI() {
        if (ShowFPS) {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(5, 5, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = "";
            if (ShowAverage) {
                if (timer > avgTick) {
                    avg = 0f;
                    for (int i = 0; i < avgFrames.Length; i++) {
                        avg += avgFrames[i];
                    }
                    avg /= 120f;
                    timer = 0f;
                }
                text = string.Format("{0:0.0} avg fps ({1:0.} fps)", avg, fps);
            } else {
                text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            }
            GUI.Label(rect, text, style);
        }
    }
}
