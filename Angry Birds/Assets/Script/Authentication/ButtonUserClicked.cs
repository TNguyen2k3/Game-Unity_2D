using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class ButtonUserClicked : MonoBehaviour
{
    public GameObject logoutButton;  // Gán nút Logout từ Inspector
    public GameObject manageButton;  // Gán nút Manage Account từ Inspector

    private bool buttonsVisible = false;
    private bool isClickable = false;
    public void OnButtonClicked()
    {
        if (isClickable){
            buttonsVisible = !buttonsVisible; // Đảo trạng thái hiển thị
            SetButtonsActive(buttonsVisible);
        }
        
    }
    void Start(){
        SetButtonsActive(false);
    }
    void Update()
    {
        if (PlayerPrefs.HasKey("nickname")) isClickable = true;
        else isClickable = false;
        // Kiểm tra nếu các nút đang hiện và người dùng click ra ngoài
        if ( buttonsVisible && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                SetButtonsActive(false); // Ẩn các nút
            }
        }
    }

    private void SetButtonsActive(bool active)
    {
        logoutButton.SetActive(active);
        manageButton.SetActive(active);
        buttonsVisible = active;
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
}
