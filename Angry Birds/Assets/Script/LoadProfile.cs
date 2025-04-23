using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using System;
[System.Serializable]
public class CompletedLevel
{
    public string levelName;
    public int score;
    public string completedAt;
}

[System.Serializable]
public class Profile 
{
    public string message;
    public string name;
    public string username;
    public string gmail;
    public int totalScore;
    public int levelPassed;
    public List<CompletedLevel> completedLevels;
}
public class LoadProfile : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text usernameText;
    public TMP_Text gmailText;
    public TMP_Text totalScoreText;
    public TMP_Text levelPassedText;
    
    // public ScrollView completedLevels;
    public GameObject levelItemPrefab;
    public Transform contentTransform;
    private string serverURL = "http://localhost:5000/auth/get-your-profile";
    // Start is called before the first frame update
    void Awake()
    {
        string nickname = PlayerPrefs.GetString("nickname");
        StartCoroutine(GetProfileData(nickname));
    }
    
    public IEnumerator GetProfileData(string nickname){
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
        Debug.Log(profile);
        nameText.text = "Name: " + profile.name;
        usernameText.text = "Username: " +"@" + profile.username;
        gmailText.text = "Gmail: " + profile.gmail;
        totalScoreText.text = "TotalScore: " + profile.totalScore.ToString();
        levelPassedText.text = "Number of levels passed: " + profile.levelPassed.ToString();
        // Clear các item cũ nếu có
        foreach (Transform child in contentTransform) {
            Destroy(child.gameObject);
        }
        // Load completed levels
        foreach (CompletedLevel level in profile.completedLevels) {
            GameObject item = Instantiate(levelItemPrefab, contentTransform);
            if (item.GetComponent<Image>()) Debug.Log(item.GetComponent<Image>().name);
            
            item.transform.Find("LevelName").GetComponent<TMP_Text>().text = "Level name: " + level.levelName;
            item.transform.Find("Score").GetComponent<TMP_Text>().text = "High score:" + level.score.ToString();

            // Format ngày
            DateTime date = DateTime.Parse(level.completedAt).ToLocalTime();
            item.transform.Find("Date").GetComponent<TMP_Text>().text = "Record date: " + date.ToString("dd/MM/yyyy HH:mm");
            item.SetActive(true);
            
        }
        // ✨ Thêm dòng này để ép Unity layout lại ngay
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform.GetComponent<RectTransform>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
