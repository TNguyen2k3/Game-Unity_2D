using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq; // Cần thư viện Newtonsoft.Json để parse JSON
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;

public class Logout : MonoBehaviour
{
    private string serverURL = "http://localhost:5000/auth/logout";
    public void OnButtonClick(){
        StartCoroutine(SendLogOutRequest());
    }
    IEnumerator SendLogOutRequest(){
        string token = PlayerPrefs.GetString("token");
        string jsonData = "{\"token\":\"" + token + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Tạo yêu cầu HTTP
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi request
        yield return request.SendWebRequest();
        // Xử lý phản hồi từ server
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            JObject jsonResponse = JObject.Parse(responseText);
            Debug.Log("Response from server: " + responseText);
            bool success = false;
            string message = jsonResponse["message"].ToString();
            if (message == "Logged out successfully")
            {
                success = true;
            }

            if (success)
            {
                
                
                // ✅ Lưu username và token vào PlayerPrefs
                
                
                PlayerPrefs.DeleteKey("token");
                PlayerPrefs.DeleteKey("nickname");
                PlayerPrefs.Save(); // Lưu thay đổi

                Debug.Log("Đăng xuất thành công! Chuyển sang scene chính...");
                SceneManager.LoadScene("Home"); // Thay bằng tên scene của bạn
            }
            else
            {
                Debug.Log("OTP không hợp lệ!");
            }
        }
        else
        {
            Debug.Log("Lỗi kết nối đến server: " + request.error);
        }
    }
}