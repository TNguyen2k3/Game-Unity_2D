using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName; // Name of the game scene you want to load after resetting the game
    void Start()
    {
        
        
        sceneName = "Level" + PlayerPrefs.GetString("current_level");
    }

    // Update is called once per frame
    public void ResetGame(){
        // Reset the game scene
        // Example: Load the level again
        if (!SceneExists(sceneName)) UnityEngine.SceneManagement.SceneManager.LoadScene("PlayCustomLevel");
        else {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // Load scene by its build index (0 is the first scene)
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
