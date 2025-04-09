using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
public class GetLevelName : MonoBehaviour
{
    public TMP_InputField levelName;
    public TMP_Text errorText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnButtonClicked(){
        if (levelName.text != ""){
            if (levelName.text.Length > 9) {
                errorText.text = "Level name mustn't longer than 9 character";
                StartCoroutine(ErrorMessage());
                return;
            }
            string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json)) {
                PlayerPrefs.SetString("current_level", levelName.text);
                PlayerPrefs.Save();
                SceneManager.LoadScene("CreateLevel");
            }
            else{
                AllLevelsData allLevels = JsonUtility.FromJson<AllLevelsData>(json);
            
                int existingIndex = allLevels.levels.FindIndex(l => l.levelName == levelName.text);
                if (existingIndex >= 0) {
                    
                    errorText.text = "This level name is really exist!";
                    StartCoroutine(ErrorMessage());
                }
                else {
                    PlayerPrefs.SetString("current_level", levelName.text);
                    SceneManager.LoadScene("CreateLevel");
                }
            }
            
        }
        else {
            errorText.text = "Level name can't be empty!";
            StartCoroutine(ErrorMessage());
        }

    }

    IEnumerator ErrorMessage(){
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        errorText.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
