using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject button;
    private bool isActive = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("nickname")){
            isActive = false;
        }
        else isActive = true;
        button.SetActive(isActive);
    }
}
