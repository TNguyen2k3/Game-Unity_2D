using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotLine : MonoBehaviour
{

    public Bird bird;
    public GameObject[] birdObject;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
     void Start()
    {
        // Lấy LineRenderer từ GameObject
        lineRenderer = GetComponent<LineRenderer>();
        //birdObject = GameObject.FindGameObjectsWithTag("Bird");
        //bird = birdObject[0].GetComponent<Bird>();
        // Đặt số lượng điểm là 3 (điểm đầu và điểm cuối)
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        birdObject = GameObject.FindGameObjectsWithTag("Bird");
        if (birdObject.Length > 0) bird = birdObject[0].GetComponent<Bird>();
         // Kiểm tra xem chim có tồn tại không
        if (bird.isDragging && lineRenderer)
        {
            lineRenderer.enabled = true;
            // Cập nhật vị trí điểm đầu và điểm cuối của LineRenderer
            lineRenderer.SetPosition(0, transform.position - new Vector3(0,-0.1f,0)); // Vị trí chặng ná
            lineRenderer.SetPosition(1, bird.transform.position);  
            lineRenderer.SetPosition(2, transform.position - new Vector3(0.1f,0.1f,0));       // Vị trí con chim (khi đang kéo)
        }
        else lineRenderer.enabled = false;
    }
}
