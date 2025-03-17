using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BombController : MonoBehaviour
{
    public GameObject explosionEffect;  // Hiệu ứng nổ (Prefab)
    public float explosionDelay = 3f;   // Thời gian nổ sau khi va chạm
    public float explosionRadius = 0.2f;  // Phạm vi nổ
    public float explosionForce = 0.2f; // Lực tối đa tại tâm vụ nổ
    public float upwardsModifier = 0.2f; // Điều chỉnh hướng lực (tùy chọn)
    Bird boom ;
    public float explosionDamage = 10;
    private bool isExploded = false;    // Biến kiểm tra xem đã phát nổ chưa
    void Start(){
        boom = GetComponent<Bird>();
    }
    void Update()
    {
        // Kiểm tra khi người chơi ấn phím hoặc click vào bom
        if (Input.GetMouseButtonDown(0) && boom.isMouseUp && !EventSystem.current.IsPointerOverGameObject())  // Nhấn chuột trái
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;  // Đặt z về 0 để tránh vấn đề về chiều sâu
            Explode();

        }
    }

    // Xử lý khi va chạm với vật khác
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded && GetComponent<Bird>().isMouseUp)  // Chỉ phát nổ nếu chưa phát nổ
        {
            //Debug.Log("Collision");
            // Bắt đầu quá trình nổ sau một khoảng thời gian nhất định
            StartCoroutine(DelayedExplosion(explosionDelay));
        }
    }

    // Hàm phát nổ ngay lập tức
    void Explode()
    {
        if (isExploded) return;  // Tránh nổ nhiều lần

        isExploded = true;
        
        // Hiển thị hiệu ứng nổ
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        // Áp dụng lực nổ
        ApplyExplosionForce();
        
        // Hủy đối tượng bom sau khi phát nổ
        // Destroy(gameObject);
        GetComponent<Bird>().Disappear();
    }

    // Hàm trì hoãn nổ sau delay giây
    IEnumerator DelayedExplosion(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    // Hàm áp dụng lực nổ cho các đối tượng xung quanh
    void ApplyExplosionForce()
    {
        // Tìm tất cả các collider trong phạm vi nổ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

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
                float forceMagnitude = explosionForce * (1 - (distance / explosionRadius));
                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                if (enemyHealth!= null)  // Kiểm tra đối tượng có EnemyHealth component hay không
                {
                    enemyHealth.health -= (1 - (distance / explosionRadius)) * explosionDamage;
                }
                // Tính lực cuối cùng
                Vector2 force = direction.normalized * forceMagnitude;

                // Áp dụng lực lên Rigidbody2D
                rb.AddForce(force + Vector2.up * upwardsModifier, ForceMode2D.Impulse);
            }
        }
    }

    // Vẽ hình tròn phạm vi nổ trong Scene view để dễ dàng kiểm tra
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}