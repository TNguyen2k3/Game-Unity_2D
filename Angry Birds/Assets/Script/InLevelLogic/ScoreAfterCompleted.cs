using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreAfterCompleted : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;
    Image finishImage;
    private GameDataManager dataManager;
    
    void Start()
    {
        dataManager = FindObjectOfType<GameDataManager>();
        finishImage = GameObject.FindGameObjectWithTag("Finish").GetComponent<Image>();
        scoreText = GetComponent<TextMeshProUGUI>();
        string level = PlayerPrefs.GetString("current_level");
        Debug.Log("level = " + level);
        string levelKey = "lv" + level;
        
        LevelDataEntry levelEntry = dataManager.gameData.levels.Find(l => l.levelKey == levelKey);
        if (levelEntry != null){
            if (levelEntry == null)
            {
                Debug.LogError("Không tìm thấy dữ liệu của level: " + levelKey);
                return;
            }
            LevelData levelData = levelEntry.levelData;
            // Debug.Log("Level: " + level + twoStarScore + "     " + threeStarScore);
            if (PlayerPrefs.HasKey("Score") && PlayerPrefs.HasKey("isWin"))
            {
                // Debug.Log("Score: " + PlayerPrefs.GetInt("Score"));
                int score = PlayerPrefs.GetInt("Score");
                scoreText.text = "Your score: " + score.ToString();
                GameObject[] stars= GameObject.FindGameObjectsWithTag("Star");
                GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
                GameObject[] pigs = GameObject.FindGameObjectsWithTag("Enemy");
                if (PlayerPrefs.GetInt("isWin") == 1) {
                    
                    foreach (GameObject bird in birds) {
                        bird.GetComponent<SpriteRenderer>().sprite = bird.GetComponent<WinAndLoseEmotion>().win;
                    }
                    foreach (GameObject pig in pigs) {
                        pig.GetComponent<SpriteRenderer>().sprite = pig.GetComponent<WinAndLoseEmotion>().lose;
                    }
                    finishImage.color = Color.yellow;
                    resultText.text = "Victory";
                    Color hexColor;
                    ColorUtility.TryParseHtmlString("#FF00FF", out hexColor);
                    if (score < levelData.twoStarScore) {
                        stars[0].GetComponent<SpriteRenderer>().color = hexColor;
                    }
                    else if (score < levelData.threeStarScore){
                        stars[0].GetComponent<SpriteRenderer>().color = hexColor;
                        stars[1].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    else {
                        stars[0].GetComponent<SpriteRenderer>().color = hexColor;
                        stars[1].GetComponent<SpriteRenderer>().color = Color.yellow;
                        stars[2].GetComponent<SpriteRenderer>().color = Color.green;
                    }
                
                }
                else {
                    resultText.text = "Defeat";
                    foreach (GameObject bird in birds) {
                        bird.GetComponent<SpriteRenderer>().sprite = bird.GetComponent<WinAndLoseEmotion>().lose;
                    }
                    foreach (GameObject pig in pigs) {
                        pig.GetComponent<SpriteRenderer>().sprite = pig.GetComponent<WinAndLoseEmotion>().win;
                    }
                    finishImage.color = Color.black;
                    scoreText.text = "Your score: " + score.ToString();
                }
                
                PlayerPrefs.DeleteKey("Score");
                PlayerPrefs.DeleteKey("isWin");
            }
            else {
                scoreText.text = "Your score: 0";  // Default score if no saved score found in PlayerPrefs. 0 means no score achieved yet. 0 is a placeholder value, you should replace it with your actual score. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging.
            }
        }
        else {
            if (PlayerPrefs.HasKey("Score") && PlayerPrefs.HasKey("isWin"))
            {
                // Debug.Log("Score: " + PlayerPrefs.GetInt("Score"));
                int score = PlayerPrefs.GetInt("Score");
                scoreText.text = "Your score: " + score.ToString();
                GameObject[] stars= GameObject.FindGameObjectsWithTag("Star");
                GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
                GameObject[] pigs = GameObject.FindGameObjectsWithTag("Enemy");
                if (PlayerPrefs.GetInt("isWin") == 1) {
                    
                    foreach (GameObject bird in birds) {
                        bird.GetComponent<SpriteRenderer>().sprite = bird.GetComponent<WinAndLoseEmotion>().win;
                    }
                    foreach (GameObject pig in pigs) {
                        pig.GetComponent<SpriteRenderer>().sprite = pig.GetComponent<WinAndLoseEmotion>().lose;
                    }
                    finishImage.color = Color.yellow;
                    resultText.text = "Victory";
                    Color hexColor;
                    ColorUtility.TryParseHtmlString("#FF00FF", out hexColor);
                    
                    stars[0].GetComponent<SpriteRenderer>().color = hexColor;
                    stars[1].GetComponent<SpriteRenderer>().color = Color.yellow;
                    stars[2].GetComponent<SpriteRenderer>().color = Color.green;
                    
                
                }
                else {
                    resultText.text = "Defeat";
                    foreach (GameObject bird in birds) {
                        bird.GetComponent<SpriteRenderer>().sprite = bird.GetComponent<WinAndLoseEmotion>().lose;
                    }
                    foreach (GameObject pig in pigs) {
                        pig.GetComponent<SpriteRenderer>().sprite = pig.GetComponent<WinAndLoseEmotion>().win;
                    }
                    finishImage.color = Color.black;
                    scoreText.text = "Your score: " + score.ToString();
                }
                
                PlayerPrefs.DeleteKey("Score");
                PlayerPrefs.DeleteKey("isWin");
            }
            else {
                scoreText.text = "Your score: 0";  // Default score if no saved score found in PlayerPrefs. 0 means no score achieved yet. 0 is a placeholder value, you should replace it with your actual score. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging. 0 can also be a placeholder value if you don't want to display any score. 0 is a good choice when you want to make the game more challenging.
            }
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
