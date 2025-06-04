using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq; // Cần thư viện Newtonsoft.Json để parse JSON
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class SignIn : MonoBehaviour
{
    public TMP_InputField gmailField;
    public TMP_InputField nicknameField;
    public TMP_InputField usernameField;
    
    private string serverURL = "http://localhost:5000/auth/register";
    // Start is called before the first frame update
    public void OnButtonClick()
    {
        string username = usernameField.text;
        string gmail = gmailField.text;
        string nickname = nicknameField.text;
        
        //Send username and OTP to server for authentication
        
        StartCoroutine(SendDataToServer(gmail, nickname, username));
    }

    IEnumerator SendDataToServer(string gmail, string nickname, string username)
    {
        // Tạo dữ liệu JSON
        string jsonData = "{\"username\":\"" + username + "\", \"gmail\":\"" + gmail + "\", \"nickname\":\"" + nickname + "\"}";
        
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi request
        yield return request.SendWebRequest();

        // Xử lý phản hồi từ server
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response from server: " + responseText);

            JObject jsonResponse = JObject.Parse(responseText);
            string success = jsonResponse["message"].ToString();

            if (success == "User registered successfully!")
            {
                
                
                // ✅ Lưu username và token vào PlayerPrefs
                // PlayerPrefs.SetString("username", username);
                // PlayerPrefs.SetString("token", token);
                // PlayerPrefs.Save(); // Lưu thay đổi

                Debug.Log("Đăng ký thành công! Chuyển sang scene đăng nhập...");
                SceneManager.LoadScene("Login"); // Thay bằng tên scene của bạn
            }
            else
            {
                Debug.Log("Đăng ký thất bại!");
            }
        }
        else
        {
            Debug.Log("Lỗi kết nối đến server: " + request.downloadHandler.text);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
