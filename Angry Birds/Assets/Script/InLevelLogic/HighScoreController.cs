using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        string level = mainCamera.GetComponent<ResultOfLevel>().level;
        if (PlayerPrefs.HasKey("highScore" + level)){
            int highScore = PlayerPrefs.GetInt("highScore" + level);
            highScoreText.text = "High Score: " + highScore;
        }
        else highScoreText.text = "High Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
