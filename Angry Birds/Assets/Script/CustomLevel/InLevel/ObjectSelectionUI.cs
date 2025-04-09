using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ObjectSelectionUI : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Danh sách object có thể chọn
    public GameObject imagePrefab; // Prefab của nút chọn object
    public Transform contentPanel; // Panel chứa danh sách button
    public Transform MainPanel;
    public List<GameObject> Enemies = new List<GameObject>();
    public ObjectSelection objectSelector; // Script chọn object
    int posY = -20;
    
    void Start()
    {
        MainPanel.gameObject.SetActive(true);  // Bật panel tạm thời
        GenerateObjectButtons();
        MainPanel.gameObject.SetActive(false); // Ẩn lại sau khi tạo xong
    }
    void Update(){
        Debug.Log("Number of enemies: " + Enemies.Count);
    }
    void GenerateObjectButtons()
    {
        
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject newImage = Instantiate(imagePrefab, contentPanel);
            newImage.SetActive(true);
            RectTransform rt = newImage.GetComponent<RectTransform>(); 
            Vector3 newPos = rt.position; // Lấy vị trí hiện tại
            newPos.y = posY - 100 * i; // Cập nhật giá trị Y
           

            rt.position = newPos; // Gán lại vị trí mới
            
            // newButton.GetComponentInChildren<TMP_Text>().text = objectPrefabs[i].GetComponent<EnemyHealth>().element;
            newImage.GetComponent<Image>().sprite = objectPrefabs[i].GetComponent<SpriteRenderer>().sprite;
            if (!newImage.GetComponent<Button>()) newImage.AddComponent<Button>();
            int index = i; // Cần tạo biến tạm để tránh lỗi delegate
            newImage.GetComponent<Button>().onClick.AddListener(() => objectSelector.SelectObject(index));
        }
    }
}