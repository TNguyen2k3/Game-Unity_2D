using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq; // Cần thư viện Newtonsoft.Json để parse JSON
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.Networking;
using TMPro;
public class GoToOnlineLevel : MonoBehaviour
{
    private string serverURL = "http://localhost:5000/auth/play";
    public TMP_Text errorMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnButtonClick(){
        if (PlayerPrefs.HasKey("nickname") && PlayerPrefs.HasKey("token")){
            StartCoroutine(SendDataToServer(PlayerPrefs.GetString("token")));
        }
    }

    IEnumerator SendDataToServer(string token){
        // Tạo yêu cầu HTTP
        UnityWebRequest request = new UnityWebRequest(serverURL, "GET");
        request.SetRequestHeader("Authorization", "Bearer " + token);
   

        // Gửi request
        yield return request.SendWebRequest();

        // Xử lý phản hồi từ server
        if (request.result == UnityWebRequest.Result.Success)
        {
           
            // Load scene hoặc cho phép chơi
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseMode");
        }
        else
        {
            StartCoroutine(ErrorMessage());
            Debug.LogError("Token không hợp lệ: " + request.error);
            Debug.Log("Server response: " + request.downloadHandler.text);
            // Hiển thị popup báo lỗi, hoặc chuyển về màn login
        }
    }

    IEnumerator ErrorMessage(){
        errorMessage.enabled = true;
        yield return new WaitForSeconds(3);
        errorMessage.enabled = false;
    }
    // Update is called once per frame
    
}
