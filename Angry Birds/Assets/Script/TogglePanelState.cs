using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TogglePanelState : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public bool isActive;
    void Start()
    {
        if (PlayerPrefs.HasKey("ActiveSetting")) isActive = PlayerPrefs.GetInt("ActiveSetting") == 1 ? true : false;
        else
        {
            isActive = false;
            PlayerPrefs.SetInt("ActiveSetting", 0);
        }
    }
    public void OnButtonClicked()
    {
        isActive = PlayerPrefs.GetInt("ActiveSetting") == 1 ? false : true; // toggle state
        panel.SetActive(isActive);
        PlayerPrefs.SetInt("ActiveSetting", isActive ? 1 : 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
