using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public static class ProfileManager {

    public static List<Profile> Profiles = new List<Profile>();
    
    public static void Load() {
        if (File.Exists(Application.persistentDataPath + "/profileData.data")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/profileData.data", FileMode.Open);
            Profiles = (List<Profile>) bf.Deserialize(file);
            file.Close();

            Debug.Log("Loaded " + Profiles.Count + " profiles.");
        } else {
            Debug.Log("No profiles loaded");
        }
    }

    public static void Save() {
        // Save Profile
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(UnityEngine.Application.persistentDataPath + "/profileData.data");
        bf.Serialize(file, Profiles);
        file.Close();
    }
}
