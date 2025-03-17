using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SmallRed : MonoBehaviour
{
    private bool isClicked = false;
    private bool isCollided = false;
     public float pushForce = 20f;
    public float pushRange = 1f; // Tầm xa có thể đẩy
    public int numPoints = 50; // Số điểm trên đường cung
    public float radius = 2f; // Bán kính của cung tròn
    public float angleRange = 90f; // Góc quét của cung (theo độ)

    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void DrawArc(Vector3 position, Rigidbody2D rb)
    {
        // Góc bắt đầu và kết thúc của cung
        
        float angleStart = -angleRange / 2f;
        float angleEnd = angleRange / 2f;
        if (rb.velocity.x < 0) {
            angleEnd -= 180;
            angleStart -= 180;
        }
        for (int i = 0; i < numPoints; i++)
        {
            // Tính toán góc cho mỗi điểm trên cung
            float angle = Mathf.Lerp(angleStart, angleEnd, (float)i / (numPoints - 1));
            float radian = angle * Mathf.Deg2Rad; // Đổi từ độ sang radian

            // Tính toán vị trí của điểm trên cung
            Vector3 pointPosition = new Vector3(
                position.x + Mathf.Cos(radian) * radius,
                position.y + Mathf.Sin(radian) * radius,
                0f
            );

            // Đặt vị trí cho điểm trong LineRenderer
            lineRenderer.SetPosition(i, pointPosition);
        }
        StartCoroutine(deleteLineRenderer());
    }
    IEnumerator deleteLineRenderer()
    {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClicked && !isCollided && Input.GetMouseButtonDown(0) && GetComponent<Bird>().isMouseUp && !EventSystem.current.IsPointerOverGameObject()){
            // Update
            isClicked = true;
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.positionCount = numPoints;
            DrawArc(transform.position, GetComponent<Rigidbody2D>());
            // Kiểm tra xem có vật thể nào trong tầm phía trước không
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, pushRange);
            foreach (var collider2D in collider2Ds)
            {
                Vector2 direction = collider2D.transform.position - transform.position;
                float distance = direction.magnitude;
                float forceMagnitude = (1 - distance/pushRange) * pushForce;
                Rigidbody2D rb = collider2D.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        
        if (GetComponent<Bird>().isMouseUp) isCollided = true;
    }
}
