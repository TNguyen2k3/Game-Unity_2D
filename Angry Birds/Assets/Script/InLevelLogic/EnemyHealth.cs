using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public double health;
    public string element = "None";
    public double maxHealth;
    public GameObject effect;
    public Sprite normalSprite; // Hình ảnh bình thường
    public Sprite bruisedSprite; // Hình ảnh khi mắt bầm tím
    public SpriteRenderer spriteRenderer;
    public float velocityAfterCollision;
    
    void Start()
    {
        maxHealth = health;
        Vector2 spriteSize = normalSprite.bounds.size;
        Vector2 desiredSize = new Vector2(2f, 2f);
        // Lấy kích thước sprite hiện tại
        Vector2 currentSpriteSize = spriteRenderer.sprite.bounds.size;
         // Tính toán PPU mới để đạt kích thước mong muốn
            float newPPU = normalSprite.pixelsPerUnit * spriteSize.x / desiredSize.x;

            // Tạo sprite mới với PPU đã điều chỉnh
            Sprite adjustedSprite = Sprite.Create(
                normalSprite.texture,
                new Rect(0, 0, normalSprite.texture.width, normalSprite.texture.height),
                normalSprite.pivot / normalSprite.rect.size,
                newPPU
            );
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Đặt hình ảnh ban đầu là bình thường
         // Lấy kích thước sprite mới
        
        
       
    }
    
    private void OnCollisionEnter2D(Collision2D collision){ 
        if (enabled){
            float damageRatio = 1f;
            Bird bird = collision.gameObject.GetComponent<Bird>();
            if (bird != null){
                if ((bird.strongElement == element && bird.strongElement != "None") || bird.strongElement == "All"){
                    damageRatio = 10f;
                }
            }
            else if (collision.gameObject.GetComponent<CloneOfBlue>() != null){
                if (element == "Ice"){
                    damageRatio = 10f;
                }
            }
            
            float damage = collision.relativeVelocity.magnitude * damageRatio; 
            
            
            if (bird){
                Debug.Log("Damage: " + damage);
                //?
                if (health > 0) velocityAfterCollision = (float) ((damage - health )/ damage + 0.0);
            }
            health -= damage;
            if (health < 0.7 * maxHealth) spriteRenderer.sprite = bruisedSprite; 
        }
        
        
        
        // if (element == "Pig") Debug.Log(spriteRenderer.sprite.name + ": " + element);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10) {
            health = 0;
            Destroy(gameObject);
            effect.SetActive(true);
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        if(health <= 0){

            health = 0;
            if (!GetComponent<BoomObject>()){
                Destroy(gameObject);
                effect.SetActive(true);
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }
        // if (element == "Pig") Debug.Log(spriteRenderer.sprite.name + ": " + element);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.gravityScale != 1){
            StartCoroutine(returnNormalGravity(rb));
        }

    }
    
    IEnumerator returnNormalGravity(Rigidbody2D rb){
        yield return new WaitForSeconds(1);
        rb.gravityScale = 1;
    }
}
