using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class SendOTP : MonoBehaviour
{
    private string serverURL = "http://localhost:5000/auth/request-otp";
    public TMP_InputField usernameField;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnButtonClick(){
        string username = usernameField.text;
        Debug.Log("Username: " + username);
        StartCoroutine(GetOTP(username));
    }

    IEnumerator GetOTP(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username không được để trống!");
            yield break; // Dừng coroutine nếu không có username
        }
        string jsonData = "{\"username\": \"" + username + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Tạo request
        using (UnityWebRequest request = new UnityWebRequest(serverURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Lỗi khi gửi OTP: " + request.error);
            }
            else
            {
                Debug.Log("Phản hồi từ server: " + request.downloadHandler.text);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
