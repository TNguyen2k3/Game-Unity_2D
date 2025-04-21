using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ChooseLevelMode : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnValueChanged(int index){
        if (dropdown.options[index].text == "Open level editor"){
            SceneManager.LoadScene("CreateLevel");
        }
        else if (dropdown.options[index].text == "Play this level") SceneManager.LoadScene("PlayCustomLevel");
        else if (dropdown.options[index].text == "Upload this level") {
            GetComponent<LevelUploader>().UploadLevel();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
