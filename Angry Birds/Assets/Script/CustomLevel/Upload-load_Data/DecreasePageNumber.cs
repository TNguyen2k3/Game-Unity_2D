using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DecreasePageNumber : MonoBehaviour
{
    public LoadSavedLevel loadSavedLevel;
    public LoadOnlineLevel loadOnlineLevel;
    public GameObject pageNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnButtonClick()
    {
        if (loadSavedLevel)
        {
            int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
            if (page > 1)
            {
                page--;
                pageNumber.GetComponentInChildren<TMP_Text>().text = page.ToString();
                loadSavedLevel.isLoaded = false; // use for update list after change page
            }
        }
        else
        {
            int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
            if (page > 1)
            {
                page--;
                pageNumber.GetComponentInChildren<TMP_Text>().text = page.ToString();
                loadOnlineLevel.isLoaded = false; // use for update list after change page
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
