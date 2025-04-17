using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    private string filePath;
    public GameData gameData;

    void Awake()
    {
        filePath = Application.dataPath + "/StreamingAssets/gamedata.json";
        LoadGameData();
    }

    // Tạo dữ liệu mặc định nếu chưa có file
    void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.Log("File not found: " + filePath);
            CreateDefaultGameData();
        }
    }

    // Tạo dữ liệu mặc định
    void CreateDefaultGameData()
    {
        gameData = new GameData();
        gameData.levels.Add(new LevelDataEntry { levelKey = "lv1", levelData = new LevelData { highScore = 0, twoStarScore = 120000, threeStarScore = 170000, starRating = 0, unlocked = true } });
        gameData.levels.Add(new LevelDataEntry { levelKey = "lv2", levelData = new LevelData { highScore = 0, twoStarScore = 80000, threeStarScore = 120000, starRating = 0, unlocked = false } });
        gameData.levels.Add(new LevelDataEntry { levelKey = "lv3", levelData = new LevelData { highScore = 0, twoStarScore = 90000, threeStarScore = 140000, starRating = 0, unlocked = false } });
        gameData.levels.Add(new LevelDataEntry { levelKey = "lv4", levelData = new LevelData { highScore = 0, twoStarScore = 100000, threeStarScore = 130000, starRating = 0, unlocked = false } });

        SaveGameData();
    }

    // Lưu dữ liệu vào file JSON
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        Debug.Log(jsonData);
        File.WriteAllText(filePath, jsonData);
    }

    // Cập nhật highScore và số sao của level
    public void UpdateLevelData(string level, int newScore, int newStar)
    {
        LevelDataEntry levelEntry = gameData.levels.Find(l => l.levelKey == level);

        if (levelEntry != null)
        {
            if (newScore > levelEntry.levelData.highScore)
            {
                levelEntry.levelData.highScore = newScore;
            }
            if (newStar > levelEntry.levelData.starRating)
            {
                levelEntry.levelData.starRating = newStar;
            }
            UnlockNextLevel(level);
            SaveGameData();
        }
        else
        {
            Debug.LogError("Không tìm thấy level: " + level);
        }
        UnlockNextLevel(level);
        SaveGameData();
    }

    // Mở khóa level tiếp theo nếu đủ điều kiện
    void UnlockNextLevel(string currentLevel)
    {
        int nextLevelIndex = int.Parse(currentLevel.Substring(2)) + 1;
        string nextLevel = "lv" + nextLevelIndex;

        LevelDataEntry currentLevelEntry = gameData.levels.Find(l => l.levelKey == currentLevel);
        LevelDataEntry nextLevelEntry = gameData.levels.Find(l => l.levelKey == nextLevel);

        if (currentLevelEntry != null && nextLevelEntry != null)
        {
            if (currentLevelEntry.levelData.starRating > 0)
            {
                nextLevelEntry.levelData.unlocked = true;
            }
        }
    }

    // Kiểm tra xem level đã mở khóa chưa
    public bool IsLevelUnlocked(string level)
    {
        LevelDataEntry levelEntry = gameData.levels.Find(l => l.levelKey == level);
        return levelEntry!= null && levelEntry.levelData.unlocked;
    }
}
