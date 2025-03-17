using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PauseButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPaused = false;
    public GameObject pauseMenuPanel; 
    public TextMeshProUGUI gameStatus;
    
    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    // void Update(){
    //     pauseButton.onClick.AddListener(TogglePause);
    // }
    // Update is called once per frame
    public void TogglePause()
    {
        
        
        if (!isPaused) {
            // Debug.Log("Clicked");
            gameStatus.text = "Resume";
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
            isPaused = true;
        }
        else {
            // Debug.Log("Clicked");
            gameStatus.text = "Pause";
            Time.timeScale = 1;
            pauseMenuPanel.SetActive(false);
            isPaused = false;
        }
        
    }
}
