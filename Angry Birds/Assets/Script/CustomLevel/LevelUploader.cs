using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Newtonsoft.Json;
public class LevelUploader : MonoBehaviour
{
    private string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
    // public Button uploadButton;
    public TMP_Text errorMessage;
    public void UploadLevel()
    {
        string json = File.ReadAllText(filePath);
        string buttonText = PlayerPrefs.GetString("current_level");
        AllLevelsData allLevelsData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<AllLevelsData>(json);
        string nickname = PlayerPrefs.GetString("nickname");
        if (buttonText != "+"){
            int existingIndex = allLevelsData.levels.FindIndex(l => l.levelName == buttonText);
            if (existingIndex >= 0 && allLevelsData.levels[existingIndex].isValidLevel) StartCoroutine(SendLevelData(allLevelsData.levels[existingIndex], nickname));
            else {
                // errorMessage.text = "";
                StartCoroutine(ErrorMessage());
            }
        }
    }
    IEnumerator ErrorMessage(){
        errorMessage.enabled = true;
        errorMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        errorMessage.enabled = false;
        errorMessage.gameObject.SetActive(false);
    }

    private IEnumerator SendLevelData(LevelCustomData levelData, string nickname)
    {
        // string json = JsonUtility.ToJson("{\"levelData\":{" + levelData + "}, \"nickname\":\"" + nickname + "\"}");
        var requestObject = new {
            levelData = levelData,   // Đưa object vào thẳng JSON
            nickname = nickname
        };

        string json = JsonConvert.SerializeObject(requestObject);
        Debug.Log("Sending JSON: " + json);
        string token = PlayerPrefs.GetString("token");
        

        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/auth/save-level", "POST");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Lỗi khi gửi: " + www.error);
        }
        else
        {
            Debug.Log("Server trả về: " + www.downloadHandler.text);
        }
    }
}
