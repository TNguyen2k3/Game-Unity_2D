using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Newtonsoft.Json.Linq;
public class LoadOnlineLevel : MonoBehaviour
{
    public Button buttonPrefab;
    public GameObject startPos;
    public GameObject Canvas;
    public List<string> availableLevels = new List<string>();
    // Start is called before the first frame update
    public void Awake()
    {
        
        StartCoroutine(LoadLevel());
        
    }

    public IEnumerator LoadLevel(){
        string token = PlayerPrefs.GetString("token");
        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/auth/get-level-list", "GET");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Lỗi khi gửi: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            try
            {
                JObject parsed = JObject.Parse(json);
                JArray levelsArray = (JArray)parsed["levels"];
                foreach (var level in levelsArray)
                {
                    string fileName = level.ToString();
                    availableLevels.Add(Path.GetFileNameWithoutExtension(fileName));
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Lỗi khi parse JSON với Newtonsoft: " + ex.Message);
            }
        }

        for(int i = 0; i < availableLevels.Count; i++){
            Button level = Instantiate(
                buttonPrefab,
                startPos.transform.position + new Vector3(200 * i, 0, 0),
                startPos.transform.rotation,
                Canvas.transform // 👈 gán nút làm con của một object nằm trong Canvas (thường là Panel)
            );
            level.GetComponentInChildren<TMP_Text>().text = availableLevels[i];
            level.gameObject.SetActive(true);
            level.interactable = true;
            level.GetComponentInChildren<TMP_Text>().fontSize = 24;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
