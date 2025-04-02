using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int highScore;
    public int twoStarScore;
    public int threeStarScore;
    public int starRating;
    public bool unlocked;
}
[System.Serializable]
public class LevelDataEntry
{
    public string levelKey;
    public LevelData levelData;
}

[System.Serializable]
public class GameData
{
    public List<LevelDataEntry> levels = new List<LevelDataEntry>();
}
