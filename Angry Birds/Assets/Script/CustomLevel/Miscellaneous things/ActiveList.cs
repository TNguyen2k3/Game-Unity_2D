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
    public void OnButtonClicked()
    {
        if (button.GetComponentInChildren<TMP_Text>().text == "+"){
            isActiveList = !isActiveList; // Đảo trạng thái hiển thị
            SetButtonsActive(isActiveList);
        }
        else {
            PlayerPrefs.SetString("current_level", button.GetComponentInChildren<TMP_Text>().text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("CreateLevel");
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
