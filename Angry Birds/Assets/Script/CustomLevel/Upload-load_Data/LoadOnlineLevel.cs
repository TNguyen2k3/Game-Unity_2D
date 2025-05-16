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
    public Transform Content;
    public GameObject pageNumber;
    public int numberPerPage = 24;

    public List<string> availableLevels = new List<string>();
    private List<GameObject> currentButtons = new List<GameObject>();
    public bool isLoaded = true;

    public void Awake()
    {
        StartCoroutine(LoadLevel());

        if (PlayerPrefs.HasKey("isOnlineLevel"))
        {
            PlayerPrefs.DeleteKey("isOnlineLevel");
        }
    }

    void Update()
    {
        if (!isLoaded)
        {
            GenerateLevelButtons();
            isLoaded = true;
        }
    }

    public IEnumerator LoadLevel()
    {
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

                availableLevels.Clear();

                foreach (var level in levelsArray)
                {
                    string fileName = level.ToString();
                    availableLevels.Add(Path.GetFileNameWithoutExtension(fileName));
                }

                GenerateLevelButtons();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Lỗi khi parse JSON với Newtonsoft: " + ex.Message);
            }
        }
    }

    private void GenerateLevelButtons()
    {
        foreach (GameObject button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();

        int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
        int i;
        int numberLevelOfRow = Mathf.CeilToInt(numberPerPage / 3f);

        for (i = numberPerPage * (page - 1); i < availableLevels.Count && i < numberPerPage * page; i++)
        {
            Button level = Instantiate(
                buttonPrefab,
                startPos.transform.position + new Vector3((200 * i) % (200 * numberLevelOfRow), -((i - numberPerPage * (page - 1)) / numberLevelOfRow) * 200, 0),
                startPos.transform.rotation,
                Content
            );
            level.GetComponentInChildren<TMP_Text>().text = availableLevels[i];
            level.gameObject.SetActive(true);
            level.interactable = true;
            level.GetComponentInChildren<TMP_Text>().fontSize = 24;

            currentButtons.Add(level.gameObject);
        }
    }
}
