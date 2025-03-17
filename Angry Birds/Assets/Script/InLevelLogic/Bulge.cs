using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Bulge : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulge;
    bool isBulge = false;
    public float bulgeRadius = 0.6f;  // Phạm vi nổ
    public float bulgeForce = 0.6f; // Lực tối đa tại tâm vụ nổ
    public float upwardsModifier = 0.4f; // Điều chỉnh hướng lực (tùy chọn)
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra khi người chơi ấn phím hoặc click vào bom
        if (Input.GetMouseButtonDown(0) && GetComponent<Bird>().isMouseUp && !EventSystem.current.IsPointerOverGameObject())  // Nhấn chuột trái
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;  // Đặt z về 0 để tránh vấn đề về chiều sâu
            //Explode();
            getBulge();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(!isBulge && GetComponent<Bird>().isMouseUp){
            StartCoroutine(bulgeAfterCollision());
        }
    }
    IEnumerator bulgeAfterCollision(){
        isBulge = true;
        yield return new WaitForSeconds(2);
        getBulge();
    }
    private void getBulge(){
        bulge.GetComponent<SpriteRenderer>().enabled = true;
        GameObject newBulge = Instantiate(bulge, transform.position, Quaternion.identity);
        newBulge.transform.localScale = new Vector3(0.5969f, 0.5969f, 0.5969f);
        newBulge.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        ApplyBulgeForce();
        Destroy(newBulge, 2f);
        Destroy(gameObject);
    }
    void ApplyBulgeForce()
    {
        // Tìm tất cả các collider trong phạm vi nổ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulgeRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Tính toán hướng từ vụ nổ đến đối tượng
                Vector2 direction = rb.transform.position - transform.position;
                float distance = direction.magnitude;
                
                // Tránh chia cho 0
                if (distance == 0f)
                {
                    distance = 0.1f;
                }

                // Tính tỷ lệ giảm dần của lực theo khoảng cách
                float forceMagnitude = 50 * (1 - (distance / bulgeRadius));
                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                
                // Tính lực cuối cùng
                Vector2 force = direction.normalized * forceMagnitude;

                // Áp dụng lực lên Rigidbody2D
                rb.AddForce(force + Vector2.up * upwardsModifier, ForceMode2D.Impulse);
            }
        }
    }
}
