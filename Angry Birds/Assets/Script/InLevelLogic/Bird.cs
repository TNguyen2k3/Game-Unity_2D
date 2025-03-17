using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Bird : MonoBehaviour
{
    //public bool isDestroyed = false;
    public string strongElement = "None";
    public int priority;
    public bool isDragging = false;  // Kiểm tra xem người chơi có đang kéo chim không
    private Vector3 originalPosition; // Vị trí ban đầu của con chim
    public float launchForce = 20f;  // Lực để đẩy con chim bay lên khi thả ra
    public float maxDistance;
    private Rigidbody2D rb;
    public bool isMouseUp = false;
    public int resolution = 30; // số điểm trên đường dẫn
    public float gravity = -9.81f; // gia tốc trọng trường
    public float jumpForce = 3f; // Lực nhảy của chim
    public float flipSpeed = 1080f; // Tốc độ quay khi lộn vòng
    private bool isJumping = false;
    
    int jumpNumber = 0;
    
    Coroutine jumpCoroutine; 
    [SerializeField] public GameObject initialPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = initialPosition.transform.position; // Lưu lại vị trí ban đầu của con chim
        //UnityEngine.Debug.Log(originalPosition);
        rb.isKinematic = true; // Đảm bảo chim không bị rơi trước khi thả ra
    }

    
    void Update()
    {
        
        if (transform.position.y <= -10){
            Disappear();
        }
        if (CompareTag("Ready") && !isJumping && !isDragging)
        {
            isJumping = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            jumpCoroutine = StartCoroutine(jump(tag));
        }
        
    }
    
    IEnumerator jump(string tag){
        yield return new WaitForSeconds(2f);
        if (!isMouseUp && tag != "Bird"){
            rb.AddForce(new Vector2 (0, jumpForce * rb.mass ), ForceMode2D.Impulse);
            jumpNumber++;
            isJumping = false;
            float rotationAmount = 0f;
            if (jumpNumber == 3){
                while (rotationAmount < 360f)
                {
                    // Tính toán góc quay trong mỗi khung hình
                    float rotationStep = flipSpeed * Time.deltaTime;
                    transform.Rotate(0, 0, rotationStep);
                    rotationAmount += rotationStep;
                    yield return null;
                }

                // Đặt lại góc về 0 sau khi lộn một vòng
                transform.rotation = Quaternion.identity;
                jumpNumber = 0;
            }
        }
    }
    // Khi người dùng nhấn chuột vào con chim
    private void OnMouseDrag()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (tag == "Bird" && !isMouseUp && !mainCamera.GetComponent<ResultOfLevel>().isFinished){
            isDragging = true;
            isMouseUp = false;
            // Nếu jumpCoroutine đang chạy, dừng nó
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
                jumpCoroutine = null;
                rb.constraints = RigidbodyConstraints2D.None;
                isJumping = false;
            }
            rb.isKinematic = true;
            // if (rb.velocity != Vector2.zero) rb.velocity = Vector2.zero;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;  // Đặt tọa độ Z của con chim thành 0 để nó chỉ di chuyển trong không gian 2D
            // Di chuyển con chim theo vị trí chuột
            Vector3 direction = originalPosition - mousePosition; 
            float length = (float) Math.Sqrt(Math.Pow(direction.x, 2) + Math.Pow(direction.y, 2));
            if (length > maxDistance) {
                direction.x = direction.x * maxDistance / length; 
                direction.y = direction.y * maxDistance / length; 
            }
            direction.z = 0;

            

            transform.position = originalPosition - direction;
        }
        
        
    }
    

    // Khi người dùng thả chuột
    private void OnMouseUp()
    {
        if (isDragging && tag == "Bird"){
            isDragging = false;
            isMouseUp = true;
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
                jumpCoroutine = null;
                rb.constraints = RigidbodyConstraints2D.None;
                isJumping = false;
            }
            GetComponent<LineRenderer>().enabled = false;
            
            rb.isKinematic = false; // Cho phép vật lý hoạt động
            
            
            Vector3 direction = originalPosition - transform.position; // Tính toán hướng bay ngược lại với hướng kéo
            float length = (float) Math.Sqrt(Math.Pow(direction.x, 2) + Math.Pow(direction.y, 2));
            if (length > maxDistance) {
                direction.x = direction.x * maxDistance / length; 
                direction.y = direction.y * maxDistance / length;
                direction.z = 0; 
            }
            
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);  // Thêm lực đẩy để chim bay
            //UnityEngine.Debug.Log(rb.velocity);
            //UnityEngine.Debug.Log("Direction: "+direction);

        }
    }


    // Reset lại vị trí chim nếu cần thiết
    public void ResetBird()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        transform.position = originalPosition;
        isMouseUp = false;
        isDragging = false;
    }
    public void Disappear(){
        Destroy(gameObject);
    }
}