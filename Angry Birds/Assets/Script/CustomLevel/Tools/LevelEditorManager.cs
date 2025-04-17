using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EditorTool
{
    Select,
    Cut,
    Delete,
    Copy,
    Paste,
    
}
public class LevelEditorManager : MonoBehaviour
{
    public SelectTool selectTool;
    public EditorTool currentTool = EditorTool.Select;
    public bool isCut = false;
    List<GameObject> cloneCopied = new List<GameObject>();
    public BirdSelectionUI birdSelectionUI;
    bool isCopy = false;
    public ObjectSelectionUI objectSelectionUI;
    public ObjectPlacer objectPlacer;
    void Update()
    {
        

       // COPY
        if ((Input.GetKeyDown(KeyCode.C) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            
                HandleToolAction(selectTool.selectedItems, Input.mousePosition, EditorTool.Copy);
            
        }

        // CUT
        if ((Input.GetKeyDown(KeyCode.X) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            
                HandleToolAction(selectTool.selectedItems, Input.mousePosition, EditorTool.Cut);
            
        }

        // DELETE
        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            
                HandleToolAction(selectTool.selectedItems, Input.mousePosition, EditorTool.Delete);
            
        }

        // PASTE
        if ((Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            HandleToolAction(selectTool.selectedItems, Input.mousePosition, EditorTool.Paste);
            
        }
    }

    void HandleToolAction(List<GameObject> obj, Vector2 mousePos, EditorTool currentTool)
    {
        switch (currentTool)
        {
            case EditorTool.Delete:
                isCopy = false;
                isCut = false;
                foreach (var o in obj){
                    
                    if (o.GetComponent<Bird>()) {
                        for (int i = 0; i < 10; i++){
                            if (birdSelectionUI.birdPositions[i].gameObject == o){
                                birdSelectionUI.birdPositions[i].gameObject = null;
                            }
                        }
                        birdSelectionUI.birdSelected.Remove(o);
                    }
                    if (o.GetComponent<EnemyHealth>()){
                        objectSelectionUI.Enemies.Remove(o);
                    }
                    Destroy(o);
                }
                selectTool.selectedItems.Clear();
                
                break;

            case EditorTool.Copy:
                if (isCut){
                    if (cloneCopied.Count > 0){
                        foreach (var i in cloneCopied){
                            Debug.Log("Cut remaining: " + i);
                            i.SetActive(true);
                        }
                    }
                    if (!isCut) cloneCopied.Clear();
                    isCut = false; 
                }
                isCopy = true;
                
                foreach (var o in obj){
                    if (o.GetComponent<EnemyHealth>()){
                        cloneCopied.Add(o);
                        selectTool.Deselect(o);
                    }
                }
                // selectTool.selectedItems.Clear();
                // GameObject clone = Instantiate(obj.gameObject, obj.transform.position + Vector3.right * 1.5f, Quaternion.identity);
                break;
            case EditorTool.Paste:
                if (cloneCopied.Count == 0) return;
                if (mousePos.x < -1) return;
                if (isCopy){
                    foreach(var o in cloneCopied){
                        if (o.GetComponent<EnemyHealth>()){
                            Debug.Log(objectSelectionUI);
                            if (objectSelectionUI.Enemies.Count < 50) objectSelectionUI.Enemies.Add(o);
                            else {
                                StartCoroutine(objectPlacer.ErrorMessage());
                                return;
                            }
                        }
                    }
                }
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                Vector3 positionOffset = mouseWorldPosition - cloneCopied[0].gameObject.transform.position;
                
                mouseWorldPosition.z = 0;  // Đảm bảo z = 0 (nếu làm việc với 2D)
                foreach(var o in cloneCopied){
                    // Tạo bản sao đối tượng tại vị trí mới
                    GameObject clone = Instantiate(o.gameObject, o.gameObject.transform.position + positionOffset, o.transform.rotation);
                    Vector3 pos = clone.transform.position;
                    pos.z = 0f;
                    clone.transform.position = pos;
                    if (isCut) clone.SetActive(true);
                    if (!isCopy) {
                        objectSelectionUI.Enemies.Remove(o);
                        objectSelectionUI.Enemies.Add(clone);
                        Destroy(o);
                    }
                }
                // add element to Enemies list
                
                if (isCut) {
                    cloneCopied.Clear();
                    selectTool.selectedItems.Clear();
                }
                
                // SetTool(EditorTool.Select);
                break;
            case EditorTool.Cut:
                isCopy = false;
                cloneCopied.Clear();
                foreach (var o in obj){
                    if (o.GetComponent<EnemyHealth>()){
                        cloneCopied.Add(o);
                        selectTool.Deselect(o);
                        o.SetActive(false);
                    }
                }
                selectTool.selectedItems.Clear();
                isCut = true;
                break;

            
        }
    }

    
    // Gọi hàm này từ UI hoặc phím tắt
    public void SetTool(EditorTool tool)
    {
        currentTool = tool;
        Debug.Log("Switched to tool: " + tool);
    }
}