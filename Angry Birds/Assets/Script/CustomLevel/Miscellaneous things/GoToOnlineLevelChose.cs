using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GoToOnlineLevelChose : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnButtonClicked(){
        PlayerPrefs.SetString("current_level", button.GetComponentInChildren<TMP_Text>().text);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("isOnlineLevel", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("PlayOnlineLevel");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
