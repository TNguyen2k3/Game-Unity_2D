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
public class Login : MonoBehaviour
{
    public TMP_Text errorMessage;
    public TMP_InputField usernameField;
    public TMP_InputField OTPField;
    private string serverURL = "http://localhost:5000/auth/verify-otp";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButtonClick()
    {
        string username = usernameField.text;
        string OTP = OTPField.text;
        
        //Send username and OTP to server for authentication
        
        StartCoroutine(SendDataToServer(username, OTP));
    }
    IEnumerator SendDataToServer(string username, string otp)
    {
        // Tạo dữ liệu JSON
        string jsonData = "{\"username\":\"" + username + "\", \"otp\":\"" + otp + "\"}";
        
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Tạo yêu cầu HTTP
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
            bool success = (bool)jsonResponse["success"];

            if (success)
            {
                string token = (string)jsonResponse["token"];
                
                // ✅ Lưu username và token vào PlayerPrefs
                Debug.Log("🔑 Token nhận được: " + token); 
                SaveNicknameFromToken(token);
                PlayerPrefs.SetString("token", token);
                PlayerPrefs.Save(); // Lưu thay đổi

                Debug.Log("Đăng nhập thành công! Chuyển sang scene chính...");
                SceneManager.LoadScene("Home"); // Thay bằng tên scene của bạn
            }
            else
            {
                Debug.Log("OTP không hợp lệ!");
            }
        }
        else
        {
            errorMessage.text = request.error;
            StartCoroutine(ErrorMessage());
            // Debug.Log("Lỗi kết nối đến server: " + request.error);
        }
    }

    public void SaveNicknameFromToken(string token)
    {
        string nickname = ExtractNickname(token);
        if (!string.IsNullOrEmpty(nickname))
        {
            PlayerPrefs.SetString("nickname", nickname);
            PlayerPrefs.Save();
            Debug.Log("Nickname saved: " + nickname);
        }
        else
        {
            Debug.LogError("Failed to extract nickname from token.");
        }
    }
    private string ExtractNickname(string token)
    {
        try
        {
            string[] parts = token.Split('.');
            if (parts.Length < 2) return null;

            string payload = parts[1]; // Lấy phần payload của JWT
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)));

            // Giải mã JSON và lấy giá trị nickname
            var payloadData = JsonUtility.FromJson<JwtPayload>(json);
            Debug.Log(payloadData.name);
            return payloadData.name;
        }
        catch (Exception e)
        {
            Debug.LogError("Error decoding JWT: " + e.Message);
            return null;
        }
    }

    private string PadBase64(string base64)
    {
        while (base64.Length % 4 != 0) base64 += "="; // Thêm padding nếu thiếu
        return base64;
    }

    [Serializable] private class JwtPayload
    {
        public string id;
        public string email;
        public string name; // Lấy giá trị "name" từ payload
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ErrorMessage(){
        errorMessage.enabled = true;
        errorMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        errorMessage.enabled = false;
        errorMessage.gameObject.SetActive(false);
    }
}
