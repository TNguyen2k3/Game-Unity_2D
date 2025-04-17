using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  // Đảm bảo bạn đã import namespace này để dùng IsPointerOverGameObject()

public class MoveScreen : MonoBehaviour
{
    // Đảm bảo MainCamera là một public field và gán vào trong Inspector
    public Camera mainCamera;
    Vector2 startPos;
    Vector2 endPos;
    Vector2 currentPos;
    Vector2 previousPos;
    public SelectTool selectTool;

    public float smoothSpeed = 10f;  // Tốc độ mượt mà của camera

    void Start()
    {
        // Gán camera nếu chưa gán trong Inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Sử dụng camera chính nếu không có gán
        }
    }

    void Update()
    {
        
        if (selectTool) {
            if (selectTool.selectBox) return;
        }
        // Khi người dùng nhấn chuột
        if (Input.GetMouseButtonDown(1))
        {
            // Kiểm tra nếu chuột đang ở trên UI, thì không làm gì
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Debug.Log("Right mouse down");
            startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            previousPos = startPos;  // Lưu lại vị trí bắt đầu
        }
        
        // Khi người dùng kéo chuột
        else if (Input.GetMouseButton(1))
        {
            endPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            // Tính toán sự di chuyển
            Vector2 movement = previousPos - endPos;
            // Debug.Log("Dragging movement: " + movement);
            // Sử dụng Lerp để làm mượt chuyển động camera
            Vector3 targetPosition = mainCamera.transform.position + (Vector3)movement;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, smoothSpeed * Time.unscaledDeltaTime);
            // Debug.Log(Time.deltaTime);
            previousPos = endPos; // Cập nhật vị trí trước cho lần kéo sau
        }
    }
}
