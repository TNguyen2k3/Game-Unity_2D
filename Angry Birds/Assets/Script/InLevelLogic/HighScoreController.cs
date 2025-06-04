using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class HighScoreController : MonoBehaviour
{
    private GameDataManager dataManager;
    private string serverURL = "http://localhost:5000/auth/get-your-profile";
    TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        dataManager = FindObjectOfType<GameDataManager>();

        string level = null;
        if (mainCamera.GetComponent<ResultOfLevel>()) level = mainCamera.GetComponent<ResultOfLevel>().level;
        if (level == null || level == "")
        {
            level = PlayerPrefs.GetString("current_level");
        }
        Debug.Log(level);
        LevelDataEntry levelData = null;
        if (dataManager) levelData = dataManager.gameData.levels.Find(l => l.levelKey == "lv" + level);

        if (levelData != null)
        {
            int highScore = levelData.levelData.highScore;
            highScoreText.text = "High Score: " + highScore;
        }
        else
        {
            string nickname = PlayerPrefs.GetString("nickname");
            StartCoroutine(GetHighScore(nickname, level));
            
        }
    }
    IEnumerator GetHighScore(string nickname, string levelName)
    {
        string jsonData = "{\"nickname\":\"" + nickname + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        string token = PlayerPrefs.GetString("token");

        // Tạo yêu cầu HTTP
        UnityWebRequest request = new UnityWebRequest(serverURL, "GET");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;
        Debug.Log(json);
        Profile profile = JsonUtility.FromJson<Profile>(json);
        // Profile profile = JsonConvert.DeserializeObject<Profile>(json);
        highScoreText.text = "High Score: " + 0;
        foreach (var i in profile.completedLevels)
        {
            if (i.levelName == levelName)
            {
                // call API to get highscore
                highScoreText.text = "High Score: " + i.score;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
