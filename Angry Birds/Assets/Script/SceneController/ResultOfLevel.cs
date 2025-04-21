using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
public class ResultOfLevel : MonoBehaviour
{
    public string level = "1";
    private GameDataManager dataManager;
    public bool isFinished = false;
    GameObject[] enemy; 
    public string finishScene;
    public string currentScene = "Level1";
    int isWin = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    int highScore;
    string levelKey;
    public AllLevelsData allLevelsData;
    public bool isCheckingPig = false;
    public int numberOfPigs = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("current_level")) level = PlayerPrefs.GetString("current_level");
        dataManager = FindObjectOfType<GameDataManager>();
        PlayerPrefs.SetString("current_level", level);
        levelKey = "lv" + level.ToString();
        if (dataManager){
            LevelDataEntry levelEntry = dataManager.gameData.levels.Find(l => l.levelKey == levelKey);
            if (levelEntry == null)
            {
                Debug.Log("Không tìm thấy dữ liệu của level: " + levelKey);
                return;
            }
            LevelData levelData = levelEntry.levelData;
            highScore = levelData.highScore;
        }
        else {
            string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("Không tìm thấy file CustomLevelData.json");
                return;
            }

            string json = File.ReadAllText(filePath);
            
            allLevelsData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<AllLevelsData>(json);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies= GameObject.FindGameObjectsWithTag("Enemy");
        
        
        if (PlayerPrefs.HasKey("isOnlineLevel")) {
            if (PlayerPrefs.GetInt("isOnlineLevel") == 1){
                if (!isCheckingPig) StartCoroutine(WaitingForLoading());
            }
        }
        else {
            
        
            
            foreach (GameObject enemy in enemies){
                if (enemy.GetComponent<EnemyHealth>().element == "Pig"){
                    numberOfPigs++;
                }
            }
            if (numberOfPigs == 0){
                //Debug.Log(scoreText.text.Substring(7));
                int score = (10000 * (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length)) + int.Parse(scoreText.text.Substring(7));
                //Debug.Log(highScoreText.text.Substring(12));
                isFinished = true;
                isWin = 1;
                StartCoroutine(WaitingFor5Sec(scoreText, isWin, finishScene));
            }
            else if (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length + GameObject.FindGameObjectsWithTag("Untagged").Length == 0){
                isFinished = true;
                // Debug.Log("111111");
                StartCoroutine(WaitingFor5Sec(scoreText, isWin, finishScene));
                
            }
        }
        
        
        
        
        if (PlayerPrefs.HasKey("isOnlineLevel")) {
            if (PlayerPrefs.GetInt("isOnlineLevel") == 1 && isCheckingPig == true){
                if (numberOfPigs == 0){
                    //Debug.Log(scoreText.text.Substring(7));
                    int score = (10000 * (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length)) + int.Parse(scoreText.text.Substring(7));
                    //Debug.Log(highScoreText.text.Substring(12));
                    isFinished = true;
                    isWin = 1;
                    StartCoroutine(WaitingFor5Sec(scoreText, isWin, finishScene));
                }
                else if (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length + GameObject.FindGameObjectsWithTag("Untagged").Length == 0){
                    isFinished = true;
                    // Debug.Log("111111");
                    StartCoroutine(WaitingFor5Sec(scoreText, isWin, finishScene));
                    
                }
            }
        }
        
        
    }
    IEnumerator WaitingForLoading(){
        
        
        while (numberOfPigs == 0)
        {
            GameObject[] enemies= GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyHealth>().element == "Pig")
                {
                    numberOfPigs++;
                }
            }
            yield return null;

            // Lặp lại kiểm tra sau mỗi frame
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            

            
        }
        isCheckingPig = true;
        Debug.Log(numberOfPigs);
    }
    IEnumerator WaitingFor5Sec(TextMeshProUGUI scoreText, int isWin, string finishScene){
        
        yield return new WaitForSeconds(5);
        int score = (10000 * (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length)) + int.Parse(scoreText.text.Substring(7));
        scoreText.text = "Score: " + score.ToString();
        if (dataManager){
            LevelDataEntry levelEntry = dataManager.gameData.levels.Find(l => l.levelKey == levelKey);
            if (isWin == 1) {
                if (score > highScore){
                    highScore = score;
                    int star = 0;
                    if (score < levelEntry.levelData.twoStarScore) {
                        star = 1;
                    }
                    else if (score < levelEntry.levelData.threeStarScore){
                        star = 2;
                    }
                    else {
                        star = 3;
                    }
                    dataManager.UpdateLevelData(levelKey, highScore, star);
                }
            }
            // PlayerPrefs.SetString("CurrentLevel", currentScene);
        }
        else {
            foreach (var i in allLevelsData.levels){
                if (i.levelName == PlayerPrefs.GetString("current_level")){
                    i.isValidLevel = true;
                    SaveToFile(i);
                    break;
                }
            }
        }
        
        PlayerPrefs.SetInt("Score", score);
        
        PlayerPrefs.SetInt("isWin", isWin);
        SceneManager.LoadScene(finishScene);
    }
    void SaveToFile(LevelCustomData newLevel)
    {
        string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
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
