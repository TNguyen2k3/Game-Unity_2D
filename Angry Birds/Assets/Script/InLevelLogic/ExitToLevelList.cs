using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToLevelList : MonoBehaviour
{
    // Start is called before the first frame update
    string sceneName = "LevelList";
    public void GoToLevelList()
    {
        SceneManager.LoadScene(sceneName);
    }
}
