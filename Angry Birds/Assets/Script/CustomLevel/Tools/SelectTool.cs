using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class SelectTool : MonoBehaviour
{
    // Start is called before the first frame update
    public bool selectBox = false;
    private Vector2 startPos;
    private Vector2 endPos;
    GameObject currentSelection;
    public RectTransform selectionBoxUI;
    public ObjectSelectionUI objectSelectionUI;
    public BirdSelectionUI birdSelectionUI;
    public List<GameObject> selectedItems = new List<GameObject>();
    public Button chooseRec;
    

    public void setSelectMode(){
        selectBox = !selectBox;
    }
    public void Select(GameObject selectedItem)
    {
        //IsSelected = true;
        selectedItems.Add(selectedItem);
        // Ví dụ: đổi màu khi được chọn
        selectedItem.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void Deselect(GameObject selectedItem)
    {
        // selectedItems.Remove(selectedItem);
        //IsSelected = false;
        selectedItem.GetComponent<SpriteRenderer>().color = Color.white;
    }

        // Update is called once per framevoid Update()
    void Update(){
        // choose one
        if(!selectBox){
            chooseRec.GetComponent<Image>().color = Color.white;
            if (Input.GetMouseButtonDown(1)) // Chuột phải
            {
                
                foreach(var item in selectedItems){
                    Deselect(item);
                }
                selectedItems.Clear();
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

                if (hit != null)
                {
                    GameObject selectable = hit.gameObject;
                    
                    if (selectable != null && (selectable.GetComponent<EnemyHealth>() || selectable.GetComponent<Bird>()))
                    {
                        
                        if (currentSelection != null && !selectedItems.Contains(selectable))
                            Deselect(currentSelection);
                        
                        currentSelection = selectable;
                        Select(currentSelection);
                    }
                }
                else
                {
                    // Click ra ngoài: bỏ chọn
                    if (currentSelection != null)
                    {
                        Deselect(currentSelection);
                        
                        currentSelection = null;
                    }
                }
            }
        }
        // select box
        else {
            chooseRec.GetComponent<Image>().color = Color.yellow;
            if (Input.GetMouseButtonDown(1))
            {
                startPos = Input.mousePosition; // Giữ screen position để dùng sau
                Vector2 uiStart = ConvertScreenToUIPosition(startPos);
                selectionBoxUI.gameObject.SetActive(true);
                DrawSelectionBox(uiStart, uiStart);
            }

            if (Input.GetMouseButton(1))
            {
                endPos = Input.mousePosition; // Cập nhật vị trí kết thúc
                Vector2 uiStart = ConvertScreenToUIPosition(startPos);
                Vector2 uiEnd = ConvertScreenToUIPosition(endPos);
                DrawSelectionBox(uiStart, uiEnd);
            }

            if (Input.GetMouseButtonUp(1))
            {
                selectionBoxUI.gameObject.SetActive(false);
                SelectObjectsWithinBounds(startPos, endPos); // Dùng screen position
            }
        }
        
    }
    Vector2 ConvertScreenToUIPosition(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            selectionBoxUI.parent as RectTransform, 
            screenPosition, 
            null, 
            out Vector2 localPoint);
        return localPoint;
    }
    void DrawSelectionBox(Vector2 startPos, Vector2 endPos)
    {
        Vector2 lowerLeft = new Vector2(
            Mathf.Min(startPos.x, endPos.x),
            Mathf.Min(startPos.y, endPos.y));
        Vector2 upperRight = new Vector2(
            Mathf.Max(startPos.x, endPos.x),
            Mathf.Max(startPos.y, endPos.y));

        Vector2 size = upperRight - lowerLeft;
        selectionBoxUI.anchoredPosition = lowerLeft;
        selectionBoxUI.sizeDelta = size;
    }

    // ✨ Chọn các đối tượng trong vùng chọn
    void SelectObjectsWithinBounds(Vector2 startPos, Vector2 endPos)
    {
        Debug.Log("SelectObjectsWithinBounds");

        // Chuyển từ UI local space sang screen space đúng cách
        Vector2 worldStart = Camera.main.ScreenToWorldPoint(startPos);
        Vector2 worldEnd = Camera.main.ScreenToWorldPoint(endPos);
        // Tìm min/max trong world space
        Vector2 min = new Vector2(Mathf.Min(worldStart.x, worldEnd.x), Mathf.Min(worldStart.y, worldEnd.y));
        Vector2 max = new Vector2(Mathf.Max(worldStart.x, worldEnd.x), Mathf.Max(worldStart.y, worldEnd.y));
       
        // Xóa danh sách cũ trước khi chọn mới
        foreach (var obj in selectedItems)
            Deselect(obj);
        selectedItems.Clear();

        // Lấy danh sách các đối tượng trong vùng chọn
        Collider2D[] hits = Physics2D.OverlapAreaAll(min, max);
        foreach (var i in hits) Debug.Log(i.gameObject.name);
        if (hits.Length == 0){
            
            selectedItems.Clear();
            
            
        }
        foreach (var hit in hits)
        {
            
            GameObject so = hit.gameObject;
            if (so != null && (so.GetComponent<EnemyHealth>() || so.GetComponent<Bird>()))
            {
                Debug.Log(so);
                Select(so);
                
            }
        }
        
        // Debug danh sách đã chọn
        foreach (var obj in selectedItems)
            Debug.Log(obj);
    }
}
