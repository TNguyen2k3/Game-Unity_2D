using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class IncreasePageNumber : MonoBehaviour
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
            int maxNumberPage = loadSavedLevel.availableLevels.Count / loadSavedLevel.numberPerPage + 1;
            int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
            if (page < maxNumberPage)
            {
                page++;
                pageNumber.GetComponentInChildren<TMP_Text>().text = page.ToString();
                loadSavedLevel.isLoaded = false; // use for update list after change page
            }
        }
        else
        {
            int maxNumberPage = loadOnlineLevel.availableLevels.Count / loadOnlineLevel.numberPerPage + 1;
            int page = int.Parse(pageNumber.GetComponentInChildren<TMP_Text>().text);
            if (page < maxNumberPage)
            {
                page++;
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
