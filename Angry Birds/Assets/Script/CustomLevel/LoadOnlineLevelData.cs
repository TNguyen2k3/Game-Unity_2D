using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
public class LoadOnlineLevelData : MonoBehaviour
{
    
    public GameObject initialPosition;
    
    public GameObject BirdParent;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadLevelData());
    }

    IEnumerator LoadLevelData(){
        string token = PlayerPrefs.GetString("token");
        UnityWebRequest www = new UnityWebRequest("http://localhost:5000/auth/get-level-data", "GET");
        string jsonData = "{\"levelName\":\"" + PlayerPrefs.GetString("current_level") + "\"}";
        
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        www.SetRequestHeader("Authorization", "Bearer " + token);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
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
            Debug.Log(json);
            LevelCustomData levelData = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<LevelCustomData>(json);

            
                
                    foreach (var bird in levelData.birds)
                    {
                        string type = "bird";
                        CreateObjectFromData(bird.data, type);
                    }

                    foreach (var enemy in levelData.enemies)
                    {
                        string type = "enemy";
                        CreateObjectFromData(enemy.data, type);
                    }
                
            
        }
    }

    void CreateObjectFromData(string data, string type)
    {
        // Tách data
        string[] parts = data.Split(new string[] { "@@" }, System.StringSplitOptions.None);
        if (parts.Length != 3)
        {
            Debug.LogWarning("Sai định dạng data: " + data);
            return;
        }

        string prefabName = parts[0];
        Vector3 pos = StringToVector3(parts[1]);
        Quaternion rot = StringToQuaternion(parts[2]);
        GameObject prefab;
        // Load prefab (nếu prefab trong Resources folder)
        if (type == "bird") prefab = Resources.Load<GameObject>("Prefabs/Alies/" + prefabName);
        else {
            prefab = Resources.Load<GameObject>("Prefabs/Enemies/" + prefabName);
            Debug.Log("Đang thử load prefab tại: " + "Prefabs/Enemies/" + prefabName);
        }
        if (prefab == null)
        {
            Debug.LogWarning("Không tìm thấy prefab: " + prefabName);
            return;
        }
        else Debug.Log(prefabName);
        GameObject temp;
        if (type == "bird") temp = Instantiate(prefab, pos, rot, BirdParent.transform);
        else temp = Instantiate(prefab, pos, rot);
        if (type == "bird") {
            
            temp.GetComponent<Bird>().initialPosition = initialPosition;
            temp.GetComponent<NormalBird>().enabled = true;
        }
        else {
            
            temp.GetComponent<EnemyHealth>().enabled = true;
        }
    }
    Vector3 StringToVector3(string s)
    {
        string[] xyz = s.Split(';');
        return new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
    }

    Quaternion StringToQuaternion(string s)
    {
        string[] xyzw = s.Split(';');
        return new Quaternion(
            float.Parse(xyzw[0]),
            float.Parse(xyzw[1]),
            float.Parse(xyzw[2]),
            float.Parse(xyzw[3])
        );
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
