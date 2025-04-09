using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class LoadSavedLevel : MonoBehaviour
{
    private string filePath;
    public List<string> availableLevels = new List<string>();
    public Button levelButton;
    public GameObject startPos;
    public GameObject Canvas;

    void Awake()
    {
        filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
        LoadLevelList();
        for(int i = 0; i < availableLevels.Count; i++){
            Button level = Instantiate(
                levelButton,
                startPos.transform.position + new Vector3(200 * i, 0, 0),
                startPos.transform.rotation,
                Canvas.transform // 👈 gán nút làm con của một object nằm trong Canvas (thường là Panel)
            );
            level.GetComponentInChildren<TMP_Text>().text = availableLevels[i];
            level.gameObject.SetActive(true);
            level.interactable = true;
            level.GetComponentInChildren<TMP_Text>().fontSize = 24;
        }
        Button addLevel = Instantiate(
            levelButton,
            startPos.transform.position + new Vector3(200 * availableLevels.Count, 0, 0),
            startPos.transform.rotation,
            Canvas.transform // 👈 gán nút làm con của một object nằm trong Canvas (thường là Panel)
        );
        addLevel.gameObject.SetActive(true);
        addLevel.interactable = true;
        
    }

    public void LoadLevelList()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Không tìm thấy file CustomLevelData.json");
            return;
        }

        string json = File.ReadAllText(filePath);
        availableLevels.Clear();
        AllLevelsData allLevelsData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<AllLevelsData>(json);

        foreach (var i in allLevelsData.levels)
        {
            if (i.levelName != null)
            {
                
                availableLevels.Add(i.levelName);
            }
        }

        Debug.Log("Danh sách level đã được load: " + string.Join(", ", availableLevels));
    }
    
}
