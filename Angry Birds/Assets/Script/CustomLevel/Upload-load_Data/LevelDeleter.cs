using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class LevelDeleter : MonoBehaviour
{
    private string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
    public LoadSavedLevel loadSavedLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DeleteLevel()
    {
        string json = File.ReadAllText(filePath);
        string buttonText = PlayerPrefs.GetString("current_level");
        AllLevelsData allLevelsData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<AllLevelsData>(json);
        int existingIndex = allLevelsData.levels.FindIndex(l => l.levelName == buttonText);
        if (existingIndex >= 0)
        {
            // delete level in server
            if (allLevelsData.levels[existingIndex].isValidLevel)
            {
                StartCoroutine(DeleteOnlineLevel(buttonText));
            }
            // delete level in local
            allLevelsData.levels.RemoveAt(existingIndex); // XÓA level tại vị trí tìm được

            // Ghi đè file JSON đã chỉnh sửa
            string updatedJson = JsonUtility.ToJson(allLevelsData, true); // `true` để đẹp mắt dễ debug
            File.WriteAllText(filePath, updatedJson);

            Debug.Log($"Level '{buttonText}' đã được xóa thành công.");
            loadSavedLevel.LoadLevel();
        }
        
    }
    IEnumerator DeleteOnlineLevel(string levelName)
    {
        string nickname = PlayerPrefs.GetString("nickname");
        string token = PlayerPrefs.GetString("token");
        Debug.Log(nickname + '_' + levelName);
        string levelJson = "{\"levelName\":\"" + nickname + '_' + levelName + "\"}";
        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/auth/delete-your-level", "DELETE");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(levelJson);
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
            if (www.responseCode == 404) Debug.Log("File này không có trên server");
            else Debug.Log("Server trả về: " + www.downloadHandler.text);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
