using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BirdPosition{
    public Vector2 pos;
    
    public GameObject gameObject;
    public BirdPosition(Vector2 pos){
        this.pos = pos;
    }
}
public class BirdSelectionUI : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Danh sách object có thể chọn
    public List<GameObject> birdSelected = new List<GameObject>();
    public GameObject imagePrefab; // Prefab của nút chọn object
    public Transform contentPanel; // Panel chứa danh sách button
    public Transform MainPanel;
    public TMP_Text ErrorText;
    float posYBird = -2f;
    float posXBird = -2.78f;
    int posY = -20;
    public BirdPosition[] birdPositions = new BirdPosition[10];
    public GameObject InitialPosition;
    
    void Start()
    {
        for (int i = 0; i < 10; i++){
            birdPositions[i] = new BirdPosition(new Vector2((float) (posXBird - i * 0.4), (float) posYBird));
        }
        MainPanel.gameObject.SetActive(true);  // Bật panel tạm thời
        GenerateObjectButtons();
        MainPanel.gameObject.SetActive(false); // Ẩn lại sau khi tạo xong
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
            newImage.GetComponent<Button>().onClick.AddListener(() => SelectObject(index));
        }
    }
    void Update(){
        Debug.Log(birdSelected.Count);
    }
    public void SelectObject(int index){
        
        if (birdSelected.Count < 10) {
            Vector2 birdPos = Vector2.zero; 
            int i;
            for (i = 0; i < 10; i++){
                
                if (birdPositions[i].gameObject == null) {
                    birdPos = birdPositions[i].pos;
                    break;
                }
                Debug.Log(birdPos);

            }
            
            GameObject currentObject = Instantiate(objectPrefabs[index], birdPos, objectPrefabs[index].transform.rotation);
            birdPositions[i].gameObject = currentObject;
            currentObject.GetComponent<Bird>().initialPosition = InitialPosition;
            currentObject.GetComponent<NormalBird>().enabled = false;
            posXBird -= 0.4f;
            birdSelected.Add(currentObject);
        }
        else StartCoroutine(ErrorMessage());
    }
    public IEnumerator ErrorMessage(){
        ErrorText.text = "Number of birds must be smaller or equal 10";
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        ErrorText.gameObject.SetActive(false);;
    }
}
