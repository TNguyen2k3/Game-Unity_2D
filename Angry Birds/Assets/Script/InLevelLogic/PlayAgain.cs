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
        if (PlayerPrefs.HasKey("CurrentLevel")) {
            sceneName = PlayerPrefs.GetString("CurrentLevel");
        }
    }

    // Update is called once per frame
    public void ResetGame(){
        // Reset the game scene
        // Example: Load the level again
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // Load scene by its build index (0 is the first scene)
        Time.timeScale = 1;
    }
}
