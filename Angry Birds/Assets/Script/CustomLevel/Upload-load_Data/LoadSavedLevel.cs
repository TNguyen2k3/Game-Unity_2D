using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class LoadSavedLevel : MonoBehaviour
{
    private string filePath = Application.dataPath + "/StreamingAssets/CustomLevelData.json";
    public List<string> availableLevels = new List<string>();
    public Button levelButton;
    public GameObject startPos;
    public GameObject pageNumber;
    public bool isLoaded = true;
    public Transform Content;
    public int numberPerPage = 24;

    private List<GameObject> currentButtons = new List<GameObject>();

    void Awake()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        LoadLevelList();
        LoadLevelButtons();
    }
    void Update()
    {
        if (!isLoaded)
        {
            LoadLevelButtons();
            isLoaded = true;
        }
    }

    void LoadLevelButtons()
    {
        // Xóa các button cũ trước khi tạo mới
        foreach (GameObject button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();

        int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
        int i;
        int numberLevelOfRow = Mathf.CeilToInt(numberPerPage / 3f);
        Debug.Log(numberLevelOfRow);
        for (i = numberPerPage * (page - 1); i < availableLevels.Count && i < numberPerPage * page; i++)
        {
            Button level = Instantiate(
                levelButton,
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

        if (i - numberPerPage * (page - 1) < numberPerPage)
        {
            Button addLevel = Instantiate(
                levelButton,
                startPos.transform.position + new Vector3((200 * i) % (200 * numberLevelOfRow), -((i - numberPerPage * (page - 1)) / numberLevelOfRow) * 200, 0),
                startPos.transform.rotation,
                Content
            );
            addLevel.gameObject.SetActive(true);
            addLevel.interactable = true;

            currentButtons.Add(addLevel.gameObject);
        }
    }

    public void LoadLevelList()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Không tìm thấy file CustomLevelData.json");
            return;
        }

        string json = File.ReadAllText(filePath);
        availableLevels.Clear();
        AllLevelsData allLevelsData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<AllLevelsData>(json);

        foreach (var i in allLevelsData.levels)
        {
            if (i.levelName != null)
            {
                availableLevels.Add(i.levelName);
            }
        }

        // Debug.Log("Danh sách level đã được load: " + string.Join(", ", availableLevels));
    }
}
