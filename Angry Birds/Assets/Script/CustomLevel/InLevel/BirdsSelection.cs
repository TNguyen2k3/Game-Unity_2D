using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsSelection : MonoBehaviour
{
    private GameObject[] objectPrefabs; // Danh sách các object có thể chọn
    private GameObject selectedObject; // Object đang được chọn
    public BirdSelectionUI birdSelectionUI;
    
    void Start(){
        objectPrefabs = birdSelectionUI.objectPrefabs;
        
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
