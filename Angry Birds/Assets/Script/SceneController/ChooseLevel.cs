using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    public string level;
    void Start(){
        level = "Level" + PlayerPrefs.GetString("current_level");
    }
    public string sceneName;
    public void ChooseALevel(){
        if (SceneExists(level)) SceneManager.LoadScene(sceneName);
        else {
            sceneName = "YourLevelList";
            SceneManager.LoadScene(sceneName);
        }
        Time.timeScale = 1;
    }
    bool SceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }

        return false;
    }
}
