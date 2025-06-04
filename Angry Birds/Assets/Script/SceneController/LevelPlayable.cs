using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelPlayable : MonoBehaviour
{
    // Start is called before the first frame update
    GameDataManager dataManager;
    public TMP_Text levelText;
    public Image lockImage;
    public Image[] starImage = new Image[3];
    private bool isLocked = true;
    string level;
    void Start()
    {
        
        dataManager = FindObjectOfType<GameDataManager>();
        level = levelText.text.Substring(6);
        LevelDataEntry levelEntry = dataManager.gameData.levels.Find(l => l.levelKey == "lv" + level);
        
        if (levelEntry == null) {
            Debug.Log("NULL");
            return;
        }
        if (levelEntry.levelData.unlocked == true){
            //unlock this level
            lockImage.gameObject.SetActive(false);
            isLocked = false;
        }
        else {
            lockImage.gameObject.SetActive(true);
            isLocked = true;

        }
        if (levelEntry.levelData.starRating > 0){
            int starRating = (int)levelEntry.levelData.starRating;
            for (int i = 0; i < starRating; i++){
                starImage[i].color = Color.yellow;
            }
        }
    }
    public void OnButtonClicked(){
        if (isLocked == false){
            PlayerPrefs.SetString("current_level", level);
            SceneManager.LoadScene("Level" + level);
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
