using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    static T instance;

    // Whether or not this object should persist when loading new scenes.
    // This should be set in the child classes Init() method.
    bool persist = false;

    public static T Instance {
        get {
            // This would only EVER be null if some other MonoBehavior requests the instance in its' Awake method.
            if (instance == null) {
                Debug.Log("[Singleton] Finding instance of '" + typeof(T).ToString() + "' object.");
                instance = FindObjectOfType(typeof(T)) as T;

                // This should only occur if 'T' hasn't been attached to any game objects in the scene.
                if (instance == null) {
                    Debug.LogError("[Singleton] No instance of " + typeof(T).ToString() + "found!");
                    GameObject singleton = new GameObject(typeof(T).ToString());
                    instance = singleton.AddComponent<T>();
                }
                instance.Init();
            }
            return instance;
        }
    }

    #region Basic getters/setters
    public bool Persist {
        get { return persist; }
        protected set { persist = value; }
    }
    #endregion

    // This will initialize our instance, if it hasn't already been prompted to do so by another MonoBehavior's Awake() requesting it first.
    void Awake() {
        if (instance == null) {
            Debug.Log("[Singleton] Initializing Singleton in Awake");
            instance = this as T;
            instance.Init();
            if (persist) {
                DontDestroyOnLoad(this.gameObject);
            }

            Debug.Log("[" + this.name + "] is persistant: " + persist);
        }
    }

    // Override this in child classes rather than Awake().
    virtual protected void Init() { }

    // Make sure no "ghost" objects are left behind when applicaton quits.
    void OnApplicationQuit() {
        instance = null;
    }
}