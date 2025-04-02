using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToAScene : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public void GoToScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
