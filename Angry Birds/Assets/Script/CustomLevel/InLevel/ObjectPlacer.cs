using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
public class ObjectPlacer : MonoBehaviour
{
    public GameObject currentObject;
    public ObjectSelection objectSelector;
    
    public TMP_Text ErrorText;
    void Start()
    {
        objectSelector = GetComponent<ObjectSelectionUI>().objectSelector;
    }

    public void Update(){
        
        // Nếu chuột đang trỏ vào UI thì không thực hiện gì cả
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (currentObject) Destroy(currentObject);
            currentObject = null;
            return;
        }
        if (Input.GetMouseButtonDown(0)){
            
            GameObject selectedPrefab = objectSelector.GetSelectedObject();
            // Debug.Log(selectedPrefab.name);
            if (selectedPrefab != null)
            {

                Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawnPos.z = 0;
                currentObject = Instantiate(selectedPrefab, spawnPos, selectedPrefab.transform.rotation);
                currentObject.GetComponent<EnemyHealth>().enabled = false;
                Debug.Log(currentObject);
            }
            
        }
        else if (Input.GetMouseButton(0) && currentObject != null) // Kéo theo chuột
        {
            Vector3 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePos.z = 0;
            if(movePos.x < -1) {
                Destroy(currentObject);
                currentObject = null;
                return;
            }
            currentObject.transform.position = movePos;
            
        }
        else if (Input.GetMouseButtonUp(0)) // Thả object
        {
            
            if (currentObject == null) return;
            Vector3 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePos.z = 0;
            if(movePos.x < -1 || movePos.x > 10 || movePos.y < -2 || movePos.y > 3) {
                Destroy(currentObject);
                currentObject = null;
                return;
            }
            if(objectSelector.objectSelectionUI.Enemies.Count < 50) objectSelector.objectSelectionUI.Enemies.Add(currentObject);
            else {
                Destroy(currentObject);
                StartCoroutine(ErrorMessage());
            }
            currentObject = null;
        }
        // Debug.Log(currentObject);
    }
    public IEnumerator ErrorMessage(){
        ErrorText.text = "Number of object must be smaller or equal 50";
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        ErrorText.gameObject.SetActive(false);;
    }
}
