using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    private GameDataManager dataManager;
    TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        dataManager = FindObjectOfType<GameDataManager>();

        string level = null;
        if (mainCamera.GetComponent<ResultOfLevel>()) level = mainCamera.GetComponent<ResultOfLevel>().level;
        if (level == null) {
            level = PlayerPrefs.GetString("current_level");
        }
        LevelDataEntry levelData = null;
        if (dataManager) dataManager.gameData.levels.Find(l => l.levelKey == "lv" + level);
        if (levelData != null){
            int highScore = levelData.levelData.highScore;
            highScoreText.text = "High Score: " + highScore;
        }
        else highScoreText.text = "High Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
