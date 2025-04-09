using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectSelection : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] objectPrefabs; // Danh sách các object có thể chọn
    private GameObject selectedObject; // Object đang được chọn
    public ObjectSelectionUI objectSelectionUI;
    
    void Start(){
        objectPrefabs = objectSelectionUI.objectPrefabs;
        
    }
    public void SelectObject(int index)
    {
        Debug.Log(index);
        
        selectedObject = objectPrefabs[index]; // Chọn object
    }
    

    public GameObject GetSelectedObject()
    {
        return selectedObject;
    }
}
