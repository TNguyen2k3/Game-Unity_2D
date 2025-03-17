using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    public string sceneName;
    public void ChooseALevel(){
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

}
