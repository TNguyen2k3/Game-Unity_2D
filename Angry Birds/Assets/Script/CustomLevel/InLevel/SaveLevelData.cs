using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro; 

[System.Serializable]
public class LevelObjectData
{
    public string data;

    public LevelObjectData(string prefabName, Vector3 position, Quaternion rotation)
    {
        data = $"{prefabName}@@{position.x};{position.y};{position.z}@@{rotation.x};{rotation.y};{rotation.z};{rotation.w}";
    }
}

[System.Serializable]
public class LevelCustomData
{
    public string levelName;
    public List<LevelObjectData> birds = new List<LevelObjectData>();
    public List<LevelObjectData> enemies = new List<LevelObjectData>();
}

[System.Serializable]
public class AllLevelsData
{
    public List<LevelCustomData> levels = new List<LevelCustomData>();
}

public class SaveLevelData : MonoBehaviour
{
    public ObjectSelectionUI objectSelectionUI;
    public BirdSelectionUI birdSelectionUI;
    

    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, JsonUtility.ToJson(new AllLevelsData(), true));
        }
    }

    public void OnButtonClicked()
    {
        string levelName = PlayerPrefs.GetString("current_level");
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Tên level không được để trống.");
            return;
        }

        // Tạo dữ liệu level
        LevelCustomData levelData = new LevelCustomData();
        levelData.levelName = levelName;

        foreach (GameObject obj in objectSelectionUI.Enemies)
        {
            levelData.enemies.Add(new LevelObjectData(
                obj.name.Replace("(Clone)", "").Trim(),
                obj.transform.position,
                obj.transform.rotation
            ));
        }

        foreach (GameObject obj in birdSelectionUI.birdSelected)
        {
            levelData.birds.Add(new LevelObjectData(
                obj.name.Replace("(Clone)", "").Trim(),
                obj.transform.position,
                obj.transform.rotation
            ));
        }

        // Ghi vào file
        SaveToFile(levelData);
    }

    void SaveToFile(LevelCustomData newLevel)
    {
        string json = File.ReadAllText(filePath);
        AllLevelsData allLevels = string.IsNullOrWhiteSpace(json) ? new AllLevelsData() : JsonUtility.FromJson<AllLevelsData>(json);

        int existingIndex = allLevels.levels.FindIndex(l => l.levelName == newLevel.levelName);
        if (existingIndex >= 0)
            allLevels.levels[existingIndex] = newLevel;
        else
            allLevels.levels.Add(newLevel);

        string newJson = JsonUtility.ToJson(allLevels, true);
        File.WriteAllText(filePath, newJson);
        Debug.Log("Level đã được lưu vào file JSON (1 trường data mỗi object).");
    }
}
