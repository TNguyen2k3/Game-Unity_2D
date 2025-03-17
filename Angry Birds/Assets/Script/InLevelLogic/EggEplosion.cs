using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEplosion : MonoBehaviour
{
    public float explosionRadius = 0.2f;  // Phạm vi nổ
    public float explosionForce = 0.2f; // Lực tối đa tại tâm vụ nổ
    public float upwardsModifier = 0.2f;
    public GameObject explosionEffect;
    public float explosionDamage = 10;
    private bool isExploded = false;  
    public bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.tag != "Bird") Explore();
    }

    void Explore(){
        if (isExploded) return;  // Tránh nổ nhiều lần

        isExploded = true;
        
        // Hiển thị hiệu ứng nổ
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        // Áp dụng lực nổ
        ApplyExplosionForce();
        
        // Hủy đối tượng bom sau khi phát nổ
        isDestroyed = true;
        Destroy(gameObject);
        
    }
    
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
                    if (enemyHealth.health < 0) enemyHealth.health = 0;
                }
                // Tính lực cuối cùng
                Vector2 force = direction.normalized * forceMagnitude;

                // Áp dụng lực lên Rigidbody2D
                rb.AddForce(force + Vector2.up * upwardsModifier, ForceMode2D.Impulse);
            }
        }
    }
}
