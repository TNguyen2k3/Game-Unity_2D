using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class WhiteThrowEgg : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject smallWhite;
    public GameObject egg;
    public GameObject[] eggs = new GameObject[2];
    int numberOfClicks = 0;
    public bool isCollided = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfClicks == 2 && !smallWhite){
            Destroy(gameObject);
        }
        if (!isCollided){
            if (numberOfClicks == 0){
                if (Input.GetMouseButtonDown(0) && GetComponent<Bird>().isMouseUp && !EventSystem.current.IsPointerOverGameObject()){
                    // Debug.Log("Throw egg");
                    numberOfClicks++;
                    eggs[0] = Instantiate(egg, transform.position - new Vector3(0, 0.3f), Quaternion.identity);
                    eggs[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    eggs[0].GetComponent<SpriteRenderer>().enabled = true;
                    eggs[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    eggs[0].GetComponent<PolygonCollider2D>().enabled = true;
                    gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 4f);
                    
                }
            }
            else if (numberOfClicks == 1){
                if (Input.GetMouseButtonDown(0) && GetComponent<Bird>().isMouseUp && !EventSystem.current.IsPointerOverGameObject()){
                    // Debug.Log("Throw egg");
                    var renderer = gameObject.GetComponent<SpriteRenderer>();
                    renderer.enabled = false;
                    // Tắt Collider (nếu cần vô hiệu hóa va chạm của cha)
                    var collider = gameObject.GetComponent<PolygonCollider2D>();
                    if (collider) collider.enabled = false;
                    var rb = gameObject.GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.velocity = new Vector2(0f, 0f);

                    numberOfClicks++;
                    eggs[1] = Instantiate(egg, transform.position - new Vector3(0, 0.3f), Quaternion.identity);
                    eggs[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    eggs[1].GetComponent<SpriteRenderer>().enabled = true;
                    eggs[1].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    eggs[1].GetComponent<PolygonCollider2D>().enabled = true;
                    smallWhite = Instantiate(smallWhite, transform.position - new Vector3(-0.3f, 0.3f), Quaternion.identity);
                    smallWhite.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    smallWhite.GetComponent<SpriteRenderer>().enabled = true;
                    smallWhite.GetComponent<PolygonCollider2D>().enabled = true;
                    smallWhite.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    smallWhite.GetComponent<Rigidbody2D>().velocity += new Vector2(5f, 10f);
                    // Destroy(gameObject);
                    
                    
                }
            }
            for (int i = 0; i < 2; i++){
                if (eggs[i]!= null && eggs[i].GetComponent<EggEplosion>().isDestroyed){
                    Destroy(eggs[i]);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (GetComponent<Bird>().isMouseUp) isCollided = true;
    }
}
