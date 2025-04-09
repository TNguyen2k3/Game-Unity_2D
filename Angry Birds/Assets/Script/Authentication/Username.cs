using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Username : MonoBehaviour
{
    public TMP_Text user;
    string username;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("nickname")){
            user.text = PlayerPrefs.GetString("nickname");
            if (user.text == "") user.text = "NULL";
        }
        else user.text = "NULL";
    }
}
