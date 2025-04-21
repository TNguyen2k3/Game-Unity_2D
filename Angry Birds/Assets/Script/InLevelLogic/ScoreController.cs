using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Tham chiếu đến Text UI để hiển thị điểm
    private int score = 0;  // Biến lưu điểm số
    public GameObject[] enemiesObj;
    private EnemyHealth[] enemies;
    void Start()
    {
        enemiesObj = GameObject.FindGameObjectsWithTag("Enemy");
        if (PlayerPrefs.HasKey("isOnlineLevel")) {
            if (PlayerPrefs.GetInt("isOnlineLevel") == 1){
                
                StartCoroutine(WaitingForLoading());
            }
        }
        else {
            enemies = new EnemyHealth[enemiesObj.Length];
            for (int i = 0; i < enemiesObj.Length; i++){
                enemies[i] = enemiesObj[i].GetComponent<EnemyHealth>();
                // Debug.Log(i , enemies[i]);
            }
            
            // Hiển thị điểm ban đầu là 0
            UpdateScoreText();
        }
        
    }
    IEnumerator WaitingForLoading(){
        GameObject[] ene;
        bool pigFound = false;

        while (!pigFound)
        {
            ene = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in ene)
            {
                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
                if (eh != null && eh.element == "Pig")
                {
                    pigFound = true;
                    break;
                }
            }

            if (!pigFound)
                yield return null; // đợi 1 frame rồi kiểm tra lại
        }

        // Sau khi đã có ít nhất 1 Pig, khởi tạo danh sách enemy
        enemiesObj = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new EnemyHealth[enemiesObj.Length];

        for (int i = 0; i < enemiesObj.Length; i++)
        {
            enemies[i] = enemiesObj[i].GetComponent<EnemyHealth>();
        }

        // Hiển thị điểm ban đầu là 0
        UpdateScoreText();
    }
    // Hàm tăng điểm và cập nhật giao diện
    public void Update()
    {
        int points = 0;
        if (enemies != null){
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].health < enemies[i].maxHealth) {
                    points += (int) ((1 - (float) (enemies[i].health/enemies[i].maxHealth)) * 5000);
                }
            }        
            score = points;
            UpdateScoreText();
        }
        
        
    }

    // Hàm cập nhật Text UI với điểm số mới
    void UpdateScoreText()
    {

        scoreText.text = "Score: " + score.ToString();
        // Debug.Log("Score: "+ score);
    }
}
