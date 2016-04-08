using System.Collections;

[System.Serializable]
public class Profile {
	public string Name = "";
    
    public int Kills = 0;
    public int Deaths = 0;
    public int Assists = 0;

    public int MatchWins = 0;
    public int GameWins = 0;

    public int GamesPlayed = 0;

    // Default Weapon/Mecha
    public WeaponType LastUsedWeapon;
    public FrameChoice LastUsedFrame;

    // Create a Profile
    public Profile(string name = "Player") {
		Name = name;

        Kills = 0;
        Deaths = 0;
        Assists = 0;

        this.LastUsedWeapon = WeaponType.BurstRifle;
        this.LastUsedFrame = FrameChoice.Legion;

        ProfileManager.Profiles.Add(this);
    }

    // Load a Profile
    public Profile(Profile profile) {
        Name = profile.Name;
        
        Kills = profile.Kills;
        Deaths = profile.Deaths;
        Assists = profile.Assists;
        
        this.LastUsedWeapon = profile.LastUsedWeapon;
        this.LastUsedFrame = profile.LastUsedFrame;
    }
}
