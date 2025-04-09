using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        
        dataManager = FindObjectOfType<GameDataManager>();
        PlayerPrefs.SetString("level", level);
        levelKey = "lv" + level.ToString();
        LevelDataEntry levelEntry = dataManager.gameData.levels.Find(l => l.levelKey == levelKey);
        if (levelEntry == null)
        {
            Debug.LogError("Không tìm thấy dữ liệu của level: " + levelKey);
            return;
        }
        LevelData levelData = levelEntry.levelData;
        highScore = levelData.highScore;
    }

    // Update is called once per frame
    void Update()
    {
        enemy= GameObject.FindGameObjectsWithTag("Enemy");
        int numberOfPigs = 0;
        foreach (GameObject enemy in enemy){
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
            Debug.Log("111111");
            StartCoroutine(WaitingFor5Sec(scoreText, isWin, finishScene));
            
        }
    }
    IEnumerator WaitingFor5Sec(TextMeshProUGUI scoreText, int isWin, string finishScene){
        
        yield return new WaitForSeconds(5);
        int score = (10000 * (GameObject.FindGameObjectsWithTag("Bird").Length + GameObject.FindGameObjectsWithTag("Ready").Length)) + int.Parse(scoreText.text.Substring(7));
        scoreText.text = "Score: " + score.ToString();
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
        PlayerPrefs.SetString("CurrentLevel", currentScene);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("isWin", isWin);
        SceneManager.LoadScene(finishScene);
    }
}
