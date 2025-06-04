using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
public class ActiveList : MonoBehaviour
{
    private bool isActiveList = false;
    public GameObject Panel;
    public Button button;
    public TMP_Dropdown chooseMode;
    public LoadSavedLevel loadSavedLevel;
    public void OnButtonClicked()
    {
        if (button)
        {
            if (button.GetComponentInChildren<TMP_Text>().text == "+")
            {   
            
                if (loadSavedLevel.availableLevels.Count >= 3)
                {
                    Debug.Log("You reach all your available level, please delete or buy some!");
                }
                else
                {
                    isActiveList = !isActiveList; // Đảo trạng thái hiển thị
                    SetButtonsActive(isActiveList);
                }
                
            }
            else
            {
                PlayerPrefs.SetString("current_level", button.GetComponentInChildren<TMP_Text>().text);
                PlayerPrefs.Save();
                chooseMode.gameObject.SetActive(true);
                chooseMode.interactable = true;
                // chooseMode.GetComponent<RectTransform>().anchoredPosition = button.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 150);
                // SceneManager.LoadScene("CreateLevel");
            }
        }
        else
        {
            isActiveList = !isActiveList; // Đảo trạng thái hiển thị
            SetButtonsActive(isActiveList);
        }

    }
    void Start(){
        // SetButtonsActive(false);
    }
    void Update()
    {
        
        // Kiểm tra nếu các nút đang hiện và người dùng click ra ngoài
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                SetButtonsActive(false); // Ẩn các nút
            }
        }
    }

    private void SetButtonsActive(bool active)
    {
        Panel.SetActive(active);
        isActiveList = active;
    }

    private bool IsPointerOverUIObject()
    {
        // Kiểm tra nếu chuột đang click vào UI
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0; // Nếu có UI nào bị trúng, trả về true
    }
    // Start is called before the first frame update
    
}
